using System;
using GINGStudio.I18N.Interface;
using GINGStudio.I18N.Util;

namespace GINStudio.I18N.Serialiser.Json
{
    public class JsonSerialiser<T> : ISerialisation<T>
    {
        public Result<T> Serialise(string s)
        {
            try {
                var t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);
                return t == null
                    ? Result<T>.NewError("Null Value")
                    : Result<T>.NewValue(t);
            } catch (Exception e)
            {
                return (Result<T>)e;
            }
        }

        public string Extension() => ".json";
    }
}