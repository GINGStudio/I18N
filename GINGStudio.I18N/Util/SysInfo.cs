using System;

namespace GINGStudio.I18N.Util
{
    internal static class SysInfo
    {
        internal static string Language => System.Globalization.CultureInfo.CurrentCulture.Name.ToLower();
    }
}