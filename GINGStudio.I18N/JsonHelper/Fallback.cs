using Newtonsoft.Json.Linq;

namespace GINGStudio.I18N
{
    public partial class JsonHelper
    {
        public static JObject Fallback(JObject master, JObject fallback)
        {
            var rst = master;
            foreach (var (key, value) in fallback)
            {
                if (value == null) continue;
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

        public static JObject Fallbacks(JObject master, params JObject?[] fallbacks)
        {
            foreach (var fallback in fallbacks)
            {
                if (fallback == null) continue;
                master = Fallback(master, fallback);
            }
            return master;
        }
    }
}