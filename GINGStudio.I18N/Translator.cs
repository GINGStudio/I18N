﻿using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using GINGStudio.I18N.Model;
using GINGStudio.I18N.Util;
using Newtonsoft.Json.Linq;

namespace GINGStudio.I18N
{
    public class Translator<T> where T : class
    {
        private string? _lang;
        private T? _value;
        private readonly string _path;
        private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>();

        private readonly ConcurrentDictionary<string, JObject> _plainJObjectCache =
            new ConcurrentDictionary<string, JObject>();

        private string GetLangPath(string lang)
            => Path.Join(_path, lang + ".json");

        private JObject? GetPlainLanguage(string? lang)
        {
            if (lang == null) return null;
            if (_plainJObjectCache.ContainsKey(lang))
                return _plainJObjectCache[lang];
            var path = GetLangPath(lang);
            if (!File.Exists(path)) return null;
            var jo = JObject.Parse(File.ReadAllText(path));
            _plainJObjectCache.TryAdd(lang, jo);
            return jo;
        }

        private readonly string[] _jsonKeywords = { "_config" };

        private JObject ApplyConfig(JObject jo)
        {
            var cfgToken = jo["_config"];
            if (cfgToken == null) return jo;
            var cfgJo = cfgToken as JObject;
            if (cfgJo == null) return jo;
            var cfgRst = JsonHelper.DeserialiseTo<Configure>(cfgJo);
            if (!cfgRst.Ok) return jo;
            var cfg = cfgRst.Unwrap();
            if (cfg.Fallback == null || cfg.Fallback.Length == 0) return jo;
            return JsonHelper.FallbacksWithIgnoreKeys(jo, _jsonKeywords,
                cfg.Fallback.Select(x => GetPlainLanguage(SysInfo.ParseToLanguage(x))).ToArray());
        }

        private bool LoadLanguage()
        {
            var lang = Language;
            if (_cache.ContainsKey(lang))
            {
                _value = _cache[lang];
                return true;
            }

            var langJo = GetPlainLanguage(lang);
            if (langJo == null) return false;
            langJo = ApplyConfig(langJo);

            var rst = JsonHelper.DeserialiseTo<T>(langJo);
            if (!rst.Ok) return false;

            _value = rst.Unwrap();
            _cache.TryAdd(lang, _value);
            return true;
        }

        private void UpdateLanguage(string lang = "")
        {
            if (lang == "") lang = SysInfo.Language;

            if (_lang == lang) return;
            var x = SysInfo.ParseToLanguage(lang);
            if (x == null) return;
            _lang = lang;
            LoadLanguage();
        }

        public string Language
        {
            get
            {
                if (string.IsNullOrEmpty(_lang)) UpdateLanguage();
                return _lang!;
            }
            set => UpdateLanguage(value);
        }

        public Translator(string path = "i18n")
        {
            _path = path;
        }
    }
}