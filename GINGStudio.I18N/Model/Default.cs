using Newtonsoft.Json;

namespace GINGStudio.I18N.Model
{
    public class Default
    {
        [JsonProperty("lang")]
        public string? Language { get; set; }
    }
}