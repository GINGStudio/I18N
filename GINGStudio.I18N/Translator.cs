using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using GINGStudio.I18N.Model;
using GINGStudio.I18N.Util;
using Newtonsoft.Json.Linq;

namespace GINGStudio.I18N
{
    public class Translator<T> where T : class
    {
        private string _lang;
        private T _value;
        private readonly string _path;
        private string _defaultLang = "";
        private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>();

        public T Text
        {
            get
            {
                if (_value != null) return _value;
                LoadLanguage();
                return _value;
            }
        }

        private readonly ConcurrentDictionary<string, JObject> _plainJObjectCache =
            new ConcurrentDictionary<string, JObject>();
        
        public bool HasDefaultLang => _defaultLang != "";
        public string DefaultLang => _defaultLang;

        private string GetLangPath(string lang)
            => Path.Combine(_path, lang + ".json");

        private JObject GetPlainLanguage(string lang)
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

        public void AutoSetLanguage() => UpdateLanguage();

        private JObject ApplyConfig(JObject jo)
        {
            var cfgToken = jo["_config"];
            if (cfgToken == null) return jo;
            var cfgJo = cfgToken as JObject;
            if (cfgJo == null) return jo;
            var cfgRst = JsonHelper.DeserialiseTo<Configure>(cfgJo);
            if (!cfgRst.Ok) return jo;
            var cfg = cfgRst.Unwrap();
            var fallback = cfg.Fallback;
            if (HasDefaultLang && !fallback.Contains(_defaultLang))
                fallback = fallback.Append(_defaultLang).ToArray();
            if (fallback == null || fallback.Length == 0) return jo;
            return JsonHelper.FallbacksWithIgnoreKeys(jo, _jsonKeywords,
                fallback.Select(x => GetPlainLanguage(SysInfo.ParseToLanguage(x))).ToArray());
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
                return _lang;
            }
            set => UpdateLanguage(value);
        }

        public Translator(string path = "i18n")
        {
            _path = path;
           LoadDefaultConfig();
        }

        private void LoadDefaultConfig()
        {
            var defaultPath = Path.Combine(_path, "default.json");
            if (!File.Exists(defaultPath)) return;
            var dft = JsonHelper.DeserialiseTo<Default>(File.ReadAllText(defaultPath));
            if (!dft.Ok) return;
            _defaultLang = dft.Unwrap().Language ?? "";
        }
    }
}
