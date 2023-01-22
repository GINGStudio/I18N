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
        
        private string GetLangPath()
        {
            return Path.Join(_path, CurrentLang + _serialisation.Extension());
        }

        private bool LoadLanguage()
        {
            var path = GetLangPath();
            if (!File.Exists(path)) return false;
            var rst = _serialisation.Serialise(File.ReadAllText(path));
            if (!rst.Ok) return false;

            Value = rst.Unwrap();
            return true;
        }

        private void SetLanguage(string lang = "")
        {
            if (lang = "") lang = SysInfo.Language;

            if (_currentLang == lang) return;
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


        public Language()
        {
        }

        public Language(ISerialisation<T> serialisation)
        {
            _serialisation = serialisation;
        }
    }
}