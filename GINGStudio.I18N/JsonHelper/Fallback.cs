using System.Linq;
using Newtonsoft.Json.Linq;

// ReSharper disable once CheckNamespace
namespace GINGStudio.I18N
{
    public partial class JsonHelper
    {
        public static JObject Fallback(JObject master, JObject fallback, string[] ignoreKeys = null)
        {
            var rst = master;
            foreach (var kvp in fallback)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                if (value == null) continue;
                if (ignoreKeys != null && ignoreKeys.Contains(key)) continue;
                if (value.Type == JTokenType.Null || value.Type == JTokenType.Comment) continue;

                if (!master.ContainsKey(key))
                {
                    rst[key] = value;
                    continue;
                }
                
                var masterToken = master[key];
                if (masterToken == null || masterToken.Type == JTokenType.Null)
                {
                    rst[key] = value;
                    continue;
                }

                switch (value.Type)
                {
                    case JTokenType.Object:
                        rst[key] = Fallback((JObject)masterToken, (JObject)value);
                        continue;
                    case JTokenType.String:
                    {
                        var v = masterToken.Value<string>();
                        if (string.IsNullOrEmpty(v)) rst[key] = value;
                        continue;
                    }
                }
            }

            return rst;
        }
        
        public static JObject Fallbacks(JObject master, params JObject[] fallbacks)
        {
            return FallbacksWithIgnoreKeys(master, null, fallbacks);
        }

        public static JObject FallbacksWithIgnoreKeys(JObject master, string[] ignoreKeys, params JObject[] fallbacks)
        {
            foreach (var fallback in fallbacks)
            {
                if (fallback == null) continue;
                master = Fallback(master, fallback, ignoreKeys);
            }
            return master;
        }
    }
}