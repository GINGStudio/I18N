using Newtonsoft.Json;

namespace GINGStudio.I18N.Model
{
    public class Configure
    {
        [JsonProperty("fallback")]
        public string[] Fallback { get; set; }
    }
}