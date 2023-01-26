using Newtonsoft.Json.Linq;

namespace GINGStudio.I18N.Test;

[TestClass]
public class JsonFallbackTest
{
    [TestMethod]
    public void TestFallback()
    {
        var j1 = @"{""name"": ""GING""}";
        var j2 = @"{""name"": ""XX"",""X"": ""GING Studio""}";
        // conv j1 to JObject
        // conv j2 to JObject
        var jo1 = JObject.Parse(j1)!;
        var jo2 = JObject.Parse(j2)!;
        var jo3 = JsonHelper.Fallback(jo1, jo2);
        Console.WriteLine(jo3.ToString());
        Assert.AreEqual(jo3["name"], jo1["name"]);
        Assert.AreEqual(jo3["X"], jo2["X"]);
    }
    
    [TestMethod]
    public void TestFallback2()
    {
        var j1 = @"{""name"": ""GING""}";
        var j2 = @"{""name"": ""XX"", ""X"": { ""v"": ""GING Studio""} }";
        // conv j1 to JObject
        // conv j2 to JObject
        var jo1 = JObject.Parse(j1)!;
        var jo2 = JObject.Parse(j2)!;
        var jo3 = JsonHelper.Fallback(jo1, jo2);
        Console.WriteLine(jo3.ToString());
        Assert.AreEqual(jo3["name"], jo1["name"]);
        var x1 = jo3["X"];
        Assert.IsNotNull(x1);
        var x2 = jo2["X"];
        Assert.IsNotNull(x2);
        Assert.AreEqual(x1.ToString(), x2.ToString());
    }
    
    [TestMethod]
    public void TestFallback3()
    {
        var j1 = @"{""name"": ""GING"", ""X"": { ""z"": ""GING Studio""} }";
        var j2 = @"{""name"": ""XX"", ""X"": { ""v"": ""GING Studio""} }";
        // conv j1 to JObject
        // conv j2 to JObject
        var jo1 = JObject.Parse(j1)!;
        var jo2 = JObject.Parse(j2)!;
        var jo3 = JsonHelper.Fallback(jo1, jo2);
        Console.WriteLine(jo3.ToString());
        Assert.AreEqual(jo3["name"], jo1["name"]);
        var x1 = jo3["X"];
        Assert.IsNotNull(x1);
        var x2 = jo2["X"];
        Assert.IsNotNull(x2);
        x1 = x1["v"];
        Assert.IsNotNull(x1);
        x2 = x2["v"];
        Assert.IsNotNull(x2);
        Assert.AreEqual(x1.ToString(), x2.ToString());
    }
}