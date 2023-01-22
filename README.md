# GINGStudio.I18N

> Current under working.

Yet Another Internationalisation Library.

## Why?

We look around the GitHub and found seems most of i18n library use `Dictionary`
and similar data structure to storage language models, which could be quite unefficient
due to `Hash Collision`. Therefore, we tried to use Serialization to handle such scenes
which will make all access would be `O(1)`.

## License

MIT