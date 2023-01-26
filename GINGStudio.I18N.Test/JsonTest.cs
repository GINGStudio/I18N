using GINGStudio.I18N.Model;
using Newtonsoft.Json;

namespace GINGStudio.I18N.Test;

[TestClass]
public class JsonTest
{
    private class LanguageModel
    {
        public string Name { get; set; }
        public string Age { get; set; }
        public string Ability { get; set; }
        public Configure _config { get; set; }
    }


    private const string PATH = "i18n";
    private readonly Translator<LanguageModel> tr;

    private string Json<T>(T v) => JsonConvert.SerializeObject(v, Formatting.Indented);


    private void Setup()
    {
        Directory.CreateDirectory(PATH);
        var zh = new LanguageModel
        {
            Name = "姓名",
            Age = "年龄",
            _config = new Configure()
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
        tr = new Translator<LanguageModel>(PATH);
    }

    [TestMethod]
    public void Test()
    {
        Assert.AreEqual("Name", tr.Text.Name);
        Assert.AreEqual("Age", tr.Text.Age);
        Assert.AreEqual("Ability", tr.Text.Ability);
        tr.Language = "zh-cn";
        Assert.AreEqual("姓名", tr.Text.Name);
        Assert.AreEqual("年龄", tr.Text.Age);
        Assert.AreEqual("Ability", tr.Text.Ability);
    }
}