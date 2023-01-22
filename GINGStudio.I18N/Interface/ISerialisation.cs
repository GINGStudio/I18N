using GINGStudio.I18N.Util;

namespace GINGStudio.I18N.Interface
{
    public interface ISerialisation<T>
    {
        public Result<T> Serialise(string s);
    }
}