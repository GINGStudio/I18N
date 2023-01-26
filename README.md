# GINGStudio.I18N

> Current under working.

Yet Another Internationalisation Library.

## Why?

We look around the GitHub and found seems most of i18n library use `Dictionary`
and similar data structure to storage language models, which could be quite unefficient
due to `Hash Collision`. Therefore, we tried to use Serialization to handle such scenes
which will make all access would be `O(1)`.

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

## License

MIT