namespace GINGStudio.I18N.Interface
{
    public interface ISerialisation<T>
    {
        public (bool Ok, T Value) Serialise(string s);
    }
}