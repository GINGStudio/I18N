namespace GINGStudio.I18N.Interface
{
    public interface ISerialisation<T>
    {
        public T Serialise(string s);
    }
}