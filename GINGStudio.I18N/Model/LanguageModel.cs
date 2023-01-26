using Newtonsoft.Json;

namespace GINGStudio.I18N.Model
{
    public class LanguageModel
    {
        [JsonProperty("_config")]
        public Configure _Config { get; set; }
    }
}