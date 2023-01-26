using System;
using GINGStudio.I18N.Util;
using Newtonsoft.Json.Linq;

namespace GINGStudio.I18N
{
    public partial class JsonHelper
    {
        public static Result<T> DeserialiseTo<T>(JObject jObject)
        {
            try
            {
                var r = jObject.ToObject<T>();
                return r == null
                    ? Result<T>.NewError("Failed to deserialise")
                    : Result<T>.NewValue(r);
            }
            catch (Exception e)
            {
                return Result<T>.NewError(e);
            }
        }
        
        public static Result<T> DeserialiseTo<T>(string json)
        {
            try
            {
                var jObject = JObject.Parse(json);
                return DeserialiseTo<T>(jObject);
            }
            catch (Exception e)
            {
                return Result<T>.NewError(e);
            }
        }
    }
}