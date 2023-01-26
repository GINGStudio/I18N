using Newtonsoft.Json.Linq;

namespace GINGStudio.I18N
{
    public partial class JsonHelper
    {
        public static T DeserialiseTo<T>(JObject jObject)
        {
            return jObject.ToObject<T>();
        }
    }
}