using System;
using System.IO;
using GINGStudio.I18N.Interface;

namespace GINGStudio.I18N
{
    public class Language<T>
    {
        private string? _currentLang;
        private T Value;
        private string _path;
        private ISerialisation<T> _serialisation;
        private string Extension;

        private string GetSystemLanguage()
        {
            return System.Globalization.CultureInfo.CurrentCulture.Name;
        }

        private string GetLangPath()
        {
            return Path.Join(_path, CurrentLang + Extension);
        }

        private bool LoadLanguage()
        {
            var path = GetLangPath();
            if (!File.Exists(path)) return false;
            var (ok, value) = _serialisation.Serialise(File.ReadAllText(path));
            if (!ok) return false;

            Value = value;
            return true;
        }

        private void SetLanguage(string lang)
        {
            if (_currentLang == lang) return;
            _currentLang = lang;
            LoadLanguage();
        }

        public string CurrentLang
        {
            get
            {
                if (_currentLang == null)
                {
                    _currentLang = GetSystemLanguage();
                }

                return _currentLang;
            }
            set { }
        }


        public Language()
        {
        }

        public Language(ISerialisation<T> serialisation)
        {
            _serialisation = serialisation;
        }
    }
}