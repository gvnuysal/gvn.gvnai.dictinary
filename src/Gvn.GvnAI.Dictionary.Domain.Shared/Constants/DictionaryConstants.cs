namespace Gvn.GvnAI.Dictionary.Domain.Shared.Constants;

public static class DictionaryConstants
{
    // Word
    public const int MaxLemmaLength = 200;

    // Sense
    public const int MaxDefinitionLength = 2000;
    public const int MaxDefinitionShortLength = 500;

    // Translation
    public const int MaxTranslationTextLength = 500;

    // Example
    public const int MaxSourceTextLength = 2000;
    public const int MaxTargetTextLength = 2000;

    // Pronunciation
    public const int MaxIpaLength = 200;
    public const int MaxVariantLength = 50;

    // Etymology
    public const int MaxEtymologyLength = 2000;

    // Lookup entities
    public const int MaxCodeLength = 20;
    public const int MaxNameLength = 100;
    public const int MaxNativeNameLength = 100;
    public const int MaxAbbreviationLength = 10;

    // User
    public const int MaxEmailLength = 256;
    public const int MaxFullNameLength = 200;

    // Paging
    public const int DefaultPageSize = 10;
}
