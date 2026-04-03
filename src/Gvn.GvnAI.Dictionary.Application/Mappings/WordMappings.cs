using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Application.Mappings;

public static class WordMappings
{
    // --- Word ---

    public static WordDto ToDto(
        this Word word,
        LanguageSummaryDto language,
        PartOfSpeechSummaryDto pos,
        Dictionary<Guid, (string Code, string Name)> registers,
        Dictionary<Guid, (string Code, string Name)> domains,
        Dictionary<Guid, LanguageSummaryDto> languages,
        Dictionary<Guid, PartOfSpeechSummaryDto> partsOfSpeech,
        Dictionary<Guid, LanguageSummaryDto>? etymologyLanguages = null)
    {
        return new WordDto(
            word.Id, word.Lemma,
            language, pos,
            word.Status, word.FrequencyRank, word.DifficultyLevel,
            word.IsCompound, word.IsIdiom, word.IsProperNoun,
            word.Senses.Select(s => s.ToDto(registers, domains, languages, partsOfSpeech)).ToList(),
            word.Pronunciations.Select(p => p.ToDto()).ToList(),
            word.Etymologies.Select(e => e.ToDto(etymologyLanguages)).ToList(),
            word.CreatedAt, word.UpdatedAt);
    }

    public static WordSummaryDto ToSummaryDto(
        this Word word,
        LanguageSummaryDto language,
        PartOfSpeechSummaryDto pos)
    {
        return new WordSummaryDto(word.Id, word.Lemma, language, pos, word.Status, word.CreatedAt);
    }

    // --- Sense ---

    public static SenseDto ToDto(
        this Sense sense,
        Dictionary<Guid, (string Code, string Name)> registers,
        Dictionary<Guid, (string Code, string Name)> domains,
        Dictionary<Guid, LanguageSummaryDto> languages,
        Dictionary<Guid, PartOfSpeechSummaryDto> partsOfSpeech)
    {
        string? registerCode = null, registerName = null;
        if (sense.RegisterId.HasValue && registers.TryGetValue(sense.RegisterId.Value, out var reg))
        {
            registerCode = reg.Code;
            registerName = reg.Name;
        }

        string? domainCode = null, domainName = null;
        if (sense.DomainId.HasValue && domains.TryGetValue(sense.DomainId.Value, out var dom))
        {
            domainCode = dom.Code;
            domainName = dom.Name;
        }

        return new SenseDto(
            sense.Id, sense.SenseNumber,
            sense.Definition, sense.DefinitionShort,
            registerCode, registerName,
            domainCode, domainName,
            sense.FrequencyRank, sense.DifficultyLevel,
            sense.Translations.Select(t => t.ToDto(languages, partsOfSpeech, registers)).ToList(),
            sense.Examples.Select(e => e.ToDto()).ToList());
    }

    // --- Translation ---

    public static TranslationDto ToDto(
        this Translation translation,
        Dictionary<Guid, LanguageSummaryDto> languages,
        Dictionary<Guid, PartOfSpeechSummaryDto> partsOfSpeech,
        Dictionary<Guid, (string Code, string Name)> registers)
    {
        var targetLanguage = languages.GetValueOrDefault(translation.TargetLanguageId)
                             ?? new LanguageSummaryDto(translation.TargetLanguageId, "??", "Unknown");

        PartOfSpeechSummaryDto? pos = translation.PartOfSpeechId.HasValue
            ? partsOfSpeech.GetValueOrDefault(translation.PartOfSpeechId.Value)
            : null;

        string? registerCode = null;
        if (translation.RegisterId.HasValue && registers.TryGetValue(translation.RegisterId.Value, out var reg))
            registerCode = reg.Code;

        return new TranslationDto(
            translation.Id,
            targetLanguage,
            translation.TranslationText,
            pos,
            registerCode,
            translation.EquivalenceType,
            translation.ConfidenceScore);
    }

    // --- Example ---

    public static ExampleDto ToDto(this Example example)
    {
        return new ExampleDto(example.Id, example.SourceText, example.TargetText, example.Source);
    }

    // --- Pronunciation ---

    public static PronunciationDto ToDto(this Pronunciation pronunciation)
    {
        return new PronunciationDto(
            pronunciation.Id, pronunciation.IpaTranscription,
            pronunciation.Variant, pronunciation.IsStandard);
    }

    // --- Etymology ---

    public static EtymologyDto ToDto(
        this Etymology etymology,
        Dictionary<Guid, LanguageSummaryDto>? languages = null)
    {
        LanguageSummaryDto? originLanguage = null;
        if (etymology.OriginLanguageId.HasValue && languages is not null)
            originLanguage = languages.GetValueOrDefault(etymology.OriginLanguageId.Value);

        return new EtymologyDto(etymology.Id, originLanguage, etymology.Text);
    }

    // --- Lookup entities ---

    public static LanguageSummaryDto ToSummaryDto(this Language language)
    {
        return new LanguageSummaryDto(language.Id, language.Code, language.Name);
    }

    public static LanguageDto ToDto(this Language language)
    {
        return new LanguageDto(language.Id, language.Code, language.Name, language.NativeName, language.Direction);
    }

    public static PartOfSpeechSummaryDto ToSummaryDto(this PartOfSpeech pos)
    {
        return new PartOfSpeechSummaryDto(pos.Id, pos.Code, pos.Name);
    }

    public static PartOfSpeechDto ToDto(this PartOfSpeech pos)
    {
        return new PartOfSpeechDto(pos.Id, pos.Code, pos.Name, pos.Abbreviation);
    }

    public static RegisterDto ToDto(this Register register)
    {
        return new RegisterDto(register.Id, register.Code, register.Name);
    }

    public static SubjectDomainDto ToDto(this SubjectDomain domain)
    {
        return new SubjectDomainDto(domain.Id, domain.Code, domain.Name);
    }
}
