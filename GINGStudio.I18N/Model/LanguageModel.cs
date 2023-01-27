using Newtonsoft.Json;

namespace GINGStudio.I18N.Model
{
    public class LanguageModel
    {
        [JsonProperty("_config")]
        // ReSharper disable once InconsistentNaming
        public Configure _Config { get; set; }
    }
}