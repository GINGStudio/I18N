# GINGStudio.I18N

Yet Another Internationalisation Library. *Unity supported*.

## Why?

We look around the GitHub and found seems most of i18n library use `Dictionary`
and similar data structure to storage language models, which could be quite unefficient
due to `Hash Collision`. Therefore, we tried to use Serialization to handle such scenes
which will make all access would be `O(1)`.

## Usage

File:

```
Project File
├── i18n
│   ├── default.json
│   ├── en-gb.json
│   └── en-us.json
└── other files
```

Model:

```csharp
class LangModel : GINGStudio.I18N.Model.LanguageModel // you can use not base LanguageModel if you want
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ItemDetail ItemDetail { get; set; }
    class ItemDetail
    {
        public string ItemName { get; set; }
    }
}
```

Load language file:

```csharp
var tr = new Translator<LangModel>("i18n"); // all translate files are in i18n folder
tr.Language = "en-gb";                      // set language to en-gb
tr.AutoSetLanguage();                       // auto set language to current system language
                                            // can do `tr.Language = "auto";` with same semantic
// Some codes
var item = new Item();
item.Name = tr.Text.ItemDetail.ItemName;    // get item name
```

## Configure

We use a special JObject `_config` to declare the configuire pairs.

```json
{
    /* Some JTokens */
    "_config": {
        /* Configure here */
    },
    /* Following JTokens */
}
```

## Fallback

We support chain-style fallback. I.e., you can fallback to as many languages as you want.
Although it is not quite recommand due to potential performance issue.

```json
/* JTokens */
"_config": {
    "fallback": [ "en-gb", "en-us" ]
}
/* JTokens */
```

## Cache

`GINGStudio.I18N` would lazy-load all needed language resources and keep them in `Cache`
data structure for faster language switch. However, to a large language model, it could
cost plenty memory. If you want, you can try set `tr.AutoClearCache = true`, this will
free all cache when language model is loaded successfully but would significantly increase
the next language model loading time.

You can also clear cache manually by `tr.ClearCache()` method.

## Unity

Yea, this library supports Unity cuz it uses `Newtonsoft.Json` instead of `System.Text.Json`.

To import this library in Unity, you should not compile the project into `dll`. Instead,
move the `GINGStudio.I18N` directory to your Unity `Assets` folder. This will allows the
library uses Unity's `Newtonsoft.Json` distribution which is able to work fine under AOT
(IL2Cpp) environment. (Seems there is no **official** AOT support from Newtonsoft)

## License

MIT
