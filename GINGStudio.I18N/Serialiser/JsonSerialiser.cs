using System;
using GINGStudio.I18N.Interface;
using GINGStudio.I18N.Util;

namespace GINGStudio.I18N.Serialiser
{
    public class JsonSerialiser<T> : ISerialisation<T>
    {
        public Result<T> Serialise(string s)
        {
            try {
                var t = System.Text.Json.JsonSerializer.Deserialize<T>(s);
                return t == null
                    ? Result<T>.NewError("Null Value")
                    : Result<T>.NewValue(t);
            } catch (Exception e)
            {
                return (Result<T>)e;
            }
        }
    }
}