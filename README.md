# GINGStudio.I18N

> Current under working.

Yet Another Internationalisation Library.

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
class LangModel
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

We support Chain-kind fallback. I.e., you can fallback to as many languages as you want.
Although it is not quite be recommand due to potential performance issue.

```json
/* JTokens */
"_config": {
    "fallback": [ "en-gb", "en-us" ]
}
/* JTokens */
```

## License

MIT