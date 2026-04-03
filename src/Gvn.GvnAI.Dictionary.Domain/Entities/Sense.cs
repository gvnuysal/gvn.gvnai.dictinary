using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Sense : Entity
{
    private readonly List<Translation> _translations = [];
    private readonly List<Example> _examples = [];

    public Guid WordId { get; private set; }
    public int SenseNumber { get; private set; }
    public string Definition { get; private set; } = null!;
    public string? DefinitionShort { get; private set; }
    public Guid? RegisterId { get; private set; }
    public Guid? DomainId { get; private set; }
    public int? FrequencyRank { get; private set; }
    public DifficultyLevel? DifficultyLevel { get; private set; }

    public IReadOnlyCollection<Translation> Translations => _translations.AsReadOnly();
    public IReadOnlyCollection<Example> Examples => _examples.AsReadOnly();

    private Sense() { }

    public static Sense Create(
        string definition, string? definitionShort, int senseNumber, Guid wordId,
        Guid? registerId, Guid? domainId,
        int? frequencyRank = null, DifficultyLevel? difficultyLevel = null)
    {
        return new Sense
        {
            WordId = wordId,
            SenseNumber = senseNumber,
            Definition = definition,
            DefinitionShort = definitionShort,
            RegisterId = registerId,
            DomainId = domainId,
            FrequencyRank = frequencyRank,
            DifficultyLevel = difficultyLevel
        };
    }

    public void Update(
        string definition, string? definitionShort,
        Guid? registerId, Guid? domainId,
        int? frequencyRank, DifficultyLevel? difficultyLevel)
    {
        Definition = definition;
        DefinitionShort = definitionShort;
        RegisterId = registerId;
        DomainId = domainId;
        FrequencyRank = frequencyRank;
        DifficultyLevel = difficultyLevel;
    }

    public void UpdateSenseNumber(int senseNumber)
    {
        SenseNumber = senseNumber;
    }

    public Translation AddTranslation(
        Guid targetLanguageId, string translationText,
        Guid? partOfSpeechId, Guid? registerId,
        EquivalenceType equivalenceType, double confidenceScore)
    {
        var translation = Translation.Create(
            Id, targetLanguageId, translationText,
            partOfSpeechId, registerId, equivalenceType, confidenceScore);
        _translations.Add(translation);
        return translation;
    }

    public void RemoveTranslation(Guid translationId)
    {
        var translation = _translations.FirstOrDefault(t => t.Id == translationId);
        if (translation is not null)
            _translations.Remove(translation);
    }

    public Example AddExample(string sourceText, string? targetText, ExampleSource source, Guid? translationId = null)
    {
        var example = Example.Create(Id, translationId, sourceText, targetText, source);
        _examples.Add(example);
        return example;
    }

    public void RemoveExample(Guid exampleId)
    {
        var example = _examples.FirstOrDefault(e => e.Id == exampleId);
        if (example is not null)
            _examples.Remove(example);
    }
}
