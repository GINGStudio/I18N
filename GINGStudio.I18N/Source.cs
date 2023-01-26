using System.IO;

namespace GINGStudio.I18N
{
    public interface ISource
    {
        string GetLangJson(string lang);
    }


    public class FileSource : ISource
    {
        public string Path { get; }
        public FileSource(string path)
        {
            Path = path;
        }

        public string GetLangJson(string lang)
        {
            var path = System.IO.Path.Combine(Path, lang + ".json");
            if (!File.Exists(path)) return null;
            return File.ReadAllText(path);
        }
    }
}