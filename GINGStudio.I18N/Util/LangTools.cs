using System.Collections.Generic;
using System.Linq;

namespace GINGStudio.I18N.Util
{
    public static class LangTools
    {
        public static string[] GetPrettyLanguageName(string[] languages)
        {
            var prettyLanguages = new string[languages.Length];
            for (var i = 0; i < languages.Length; i++)
            {
                var language = languages[i];
                prettyLanguages[i] = GetPrettyLanguageName(language);
            }
            return prettyLanguages;
        }

        public static string GetPrettyLanguageName(string lang)
        {
            var culture = new System.Globalization.CultureInfo(lang);
            return culture.NativeName;
        }
        
        public static Dictionary<string, string> GetLangPairs(string[] languages)
        {
            var langPairs = new Dictionary<string, string>();
            foreach (var language in languages)
            {
                var prettyLanguage = GetPrettyLanguageName(language);
                langPairs.Add(language, prettyLanguage);
            }
            return langPairs;
        }
    }
}