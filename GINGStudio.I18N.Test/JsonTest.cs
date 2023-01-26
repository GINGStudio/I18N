using GINGStudio.I18N.Model;
using Newtonsoft.Json;

namespace GINGStudio.I18N.Test;

[TestClass]
public class JsonTest
{
    private class LanguageModel : Model.LanguageModel
    {
        public string? Name { get; set; }
        public string? Age { get; set; }
        public string? Ability { get; set; }
    }


    private const string PATH = "i18n";
    private readonly Translator<LanguageModel> _tr;

    private string Json<T>(T v) => JsonConvert.SerializeObject(v, Formatting.Indented);


    private void Setup()
    {
        Directory.CreateDirectory(PATH);
        var zh = new LanguageModel
        {
            Name = "姓名",
            Age = "年龄",
            _Config = new Configure()
            {
                Fallback = new[] { "en" }
            }
        };
        var en = new LanguageModel
        {
            Name = "Name",
            Age = "Age",
            Ability = "Ability"
        };
        var dft = new Default
        {
            Language = "en-gb"
        };
        File.WriteAllText(Path.Combine(PATH, "zh-cn.json"), Json(zh));
        File.WriteAllText(Path.Combine(PATH, "en-gb.json"), Json(en));
        File.WriteAllText(Path.Combine(PATH, "default.json"), Json(dft));
    }

    public JsonTest()
    {
        Setup();
        _tr = new Translator<LanguageModel>(PATH);
    }

    [TestMethod]
    public void Test()
    {
        Console.WriteLine(_tr.Language);
        Assert.AreEqual("Name", _tr.Text.Name);
        Assert.AreEqual("Age", _tr.Text.Age);
        Assert.AreEqual("Ability", _tr.Text.Ability);
        _tr.Language = "zh-cn";
        Assert.AreEqual("姓名", _tr.Text.Name);
        Assert.AreEqual("年龄", _tr.Text.Age);
        Assert.AreEqual("Ability", _tr.Text.Ability);
    }
}