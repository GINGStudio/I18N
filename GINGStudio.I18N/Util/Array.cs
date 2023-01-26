namespace GINGStudio.I18N.Util
{
    public static class Array
    {
        public static T[] Append<T>(this T[]? array, T value)
        {
            if (array == null) return new[] {value};
            var newArray = new T[array.Length + 1];
            array.CopyTo(newArray, 0);
            newArray[^1] = value;
            return newArray;
        }
    }
}