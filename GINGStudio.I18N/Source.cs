using System.IO;
using System.Linq;
// ReSharper disable RedundantDefaultMemberInitializer

namespace GINGStudio.I18N
{
    public interface ISource
    {
        string GetLangJson(string lang);
        string[] SupportedLanguages { get; }
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

        private string[] _supportedLanguages = null;

        public string[] SupportedLanguages
        {
            get {
                if (_supportedLanguages != null) return _supportedLanguages;
                _supportedLanguages = Directory.GetFiles(Path, "*.json")
                    .Select(System.IO.Path.GetFileNameWithoutExtension)
                    .Where(x => x != "default")
                    .ToArray();
                return _supportedLanguages;
            }
        }
    }
}