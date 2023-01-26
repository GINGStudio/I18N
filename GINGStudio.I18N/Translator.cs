using System.Collections.Concurrent;
using System.IO;
using GINGStudio.I18N.Util;

namespace GINGStudio.I18N
{
    public class Translator<T> where T : class
    {
        private string? _lang;
        private T? _value;
        private readonly string _path;
        private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>();

        private string GetLangPath(string lang)
            => Path.Join(_path, lang + ".json");

        private bool LoadLanguage()
        {
            var lang = Language;
            if (_cache.ContainsKey(lang))
            {
                _value = _cache[lang];
                return true;
            }

            var path = GetLangPath(lang);
            if (!File.Exists(path)) return false;
            var rst = JsonHelper.DeserialiseTo<T>(File.ReadAllText(path));
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