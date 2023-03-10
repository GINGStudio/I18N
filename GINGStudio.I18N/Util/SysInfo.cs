namespace GINGStudio.I18N.Util
{
    internal static class SysInfo
    {
        internal static string Language => System.Globalization.CultureInfo.CurrentCulture.Name.ToLower();

        /// <summary>
        /// Parse from lang string
        /// </summary>
        /// <param name="lang"></param>
        /// <returns>if is null then not valid!</returns>
        internal static string ParseToLanguage(string lang)
        {
            try
            {
                lang = lang.ToLower();
                if (lang == "auto" || lang == "local" || lang == "") return Language;
                return new System.Globalization.CultureInfo(lang).Name.ToLower();
            }
            catch
            {
                return null;
            }
        }
    }
}