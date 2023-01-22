using System;
using GINGStudio.I18N.Interface;

namespace GINGStudio.I18N.Serialiser
{
    public class JsonSerialiser<T> : ISerialisation<T>
    {
        public (bool Ok, T Value) Serialise(string s)
        {
            try {
                var t = System.Text.Json.JsonSerializer.Deserialize<T>(s);
                return (t != null, t);
            } catch (Exception e)
            {
                return (false, default);
            }
        }
    }
}