using System.Collections.Concurrent;
using System.Linq;
using GINGStudio.I18N.Model;
using GINGStudio.I18N.Util;
using Newtonsoft.Json.Linq;
// ReSharper disable RedundantDefaultMemberInitializer

namespace GINGStudio.I18N
{
    public class Translator<T> where T : class
    {
        private string _lang;
        private T _value;
        private ISource _source;
        private string _defaultLang = "";
        private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>();
        private bool _autoClearCache = false;
        public bool AutoClearCache
        {
            get => _autoClearCache;
            set
            {
                if (_autoClearCache == value) return;
                _autoClearCache = value;
                if (value) ClearCache();
            }
        }

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

        private JObject GetPlainLanguage(string lang)
        {
            if (lang == null) return null;
            if (_plainJObjectCache.ContainsKey(lang))
                return _plainJObjectCache[lang];
            var json = _source.GetLangJson(lang);
            if (json == null) return null;
            var jo = JObject.Parse(json);
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

        // ReSharper disable once UnusedMethodReturnValue.Local
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
           
            if (!_autoClearCache)  _cache.TryAdd(lang, _value);
            else ClearCache();
            return true;
        }

        private void UpdateLanguage(string lang = "")
        {
            if (lang == "") lang = SysInfo.Language;
            if (lang == "") lang = _defaultLang;
            if (lang == "") lang = "en-gb";

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
            _source = new FileSource(path);
            LoadDefaultConfig();
        }
        
        public Translator(ISource source)
        {
            _source = source;
            LoadDefaultConfig();
        }

        private void LoadDefaultConfig()
        {
            var json = _source.GetLangJson("default");
            if (json == null) return;
            var dft = JsonHelper.DeserialiseTo<Default>(json);
            if (!dft.Ok) return;
            _defaultLang = dft.Unwrap().Language ?? "";
        }
        
        public void ClearCache()
        {
            _cache.Clear();
            _plainJObjectCache.Clear();
        }
        
        public string[] SupportedLanguages => _source.SupportedLanguages;
    }
}