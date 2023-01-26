using System.Collections.Concurrent;
using System.IO;
using GINGStudio.I18N.Util;

namespace GINGStudio.I18N
{
    public class Language<T>
    {
        private string? _currentLang;
        private T Value;
        private readonly string _path;
        private readonly ConcurrentDictionary<string, T> _cache = new ConcurrentDictionary<string, T>();

        private string GetLangPath(string lang)
            => Path.Join(_path, lang + ".json");

        private bool LoadLanguage()
        {
            var lang = CurrentLang;
            if (_cache.ContainsKey(lang))
            {
                Value = _cache[lang];
                return true;
            }

            var path = GetLangPath(lang);
            if (!File.Exists(path)) return false;
            var rst = JsonHelper.DeserialiseTo<T>(File.ReadAllText(path));
            if (!rst.Ok) return false;

            Value = rst.Unwrap();
            _cache.TryAdd(lang, Value);
            return true;
        }

        private void SetLanguage(string lang = "")
        {
            if (lang == "") lang = SysInfo.Language;

            if (_currentLang == lang) return;
            var x = SysInfo.ParseToLanguage(lang);
            if (x == null) return;
            _currentLang = lang;
            LoadLanguage();
        }

        public string CurrentLang
        {
            get
            {
                if (string.IsNullOrEmpty(_currentLang)) SetLanguage();
                return _currentLang;
            }
            set => SetLanguage(value);
        }

        public Language(string path = "i18n")
        {
            _path = path;
        }
    }
}