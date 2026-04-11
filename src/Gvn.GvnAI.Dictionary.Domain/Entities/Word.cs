using Gvn.GvnAI.Dictionary.Domain.Events;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Aggregates;
using Gvn.GvnFramework.Domain.Common;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Word : AggregateRoot, ISoftDeletable
{
    private readonly List<Sense> _senses = [];
    private readonly List<Pronunciation> _pronunciations = [];
    private readonly List<Etymology> _etymologies = [];

    public string Lemma { get; private set; } = null!;
    public Guid UserId { get; private set; }
    public Guid LanguageId { get; private set; }
    public string? Synonyms { get; private set; }
    public string? Antonyms { get; private set; }
    public Guid PartOfSpeechId { get; private set; }
    public int? FrequencyRank { get; private set; }
    public DifficultyLevel? DifficultyLevel { get; private set; }
    public bool IsCompound { get; private set; }
    public bool IsIdiom { get; private set; }
    public bool IsProperNoun { get; private set; }
    public WordStatus Status { get; private set; }

    // ISoftDeletable
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }

    // Collections
    public IReadOnlyCollection<Sense> Senses => _senses.AsReadOnly();
    public IReadOnlyCollection<Pronunciation> Pronunciations => _pronunciations.AsReadOnly();
    public IReadOnlyCollection<Etymology> Etymologies => _etymologies.AsReadOnly();

    private Word() { }

    public static Word Create(string lemma, Guid languageId, Guid partOfSpeechId, Guid userId)
    {
        var word = new Word
        {
            Lemma = lemma.Trim(),
            UserId = userId,
            LanguageId = languageId,
            PartOfSpeechId = partOfSpeechId,
            Status = WordStatus.Pending
        };

        return word;
    }

    public void Update(
        string lemma, Guid languageId, Guid partOfSpeechId,
        int? frequencyRank, DifficultyLevel? difficultyLevel,
        bool isCompound, bool isIdiom, bool isProperNoun)
    {
        Lemma = lemma.Trim();
        LanguageId = languageId;
        PartOfSpeechId = partOfSpeechId;
        FrequencyRank = frequencyRank;
        DifficultyLevel = difficultyLevel;
        IsCompound = isCompound;
        IsIdiom = isIdiom;
        IsProperNoun = isProperNoun;
    }

    public void UpdateSynonyms(string? synonyms, string? antonyms)
    {
        Synonyms = synonyms;
        Antonyms = antonyms;
    }

    public Sense AddSense(
        string definition, string? definitionShort,
        Guid? registerId, Guid? domainId,
        int? frequencyRank, DifficultyLevel? difficultyLevel)
    {
        var senseNumber = _senses.Count > 0 ? _senses.Max(s => s.SenseNumber) + 1 : 1;
        var sense = Sense.Create(definition, definitionShort, senseNumber, Id, registerId, domainId, frequencyRank, difficultyLevel);
        _senses.Add(sense);
        return sense;
    }

    public void RemoveSense(Guid senseId)
    {
        var sense = _senses.FirstOrDefault(s => s.Id == senseId);
        if (sense is not null)
            _senses.Remove(sense);
    }

    public void ReorderSenses(List<Guid> orderedSenseIds)
    {
        for (var i = 0; i < orderedSenseIds.Count; i++)
        {
            var sense = _senses.FirstOrDefault(s => s.Id == orderedSenseIds[i]);
            sense?.UpdateSenseNumber(i + 1);
        }
    }

    public void AddPronunciation(string ipa, string? variant, bool isStandard)
    {
        _pronunciations.Add(Pronunciation.Create(ipa, variant, isStandard, Id));
    }

    public void AddEtymology(Guid? originLanguageId, string text)
    {
        _etymologies.Add(Etymology.Create(originLanguageId, text, Id));
    }

    public void Enrich(
        List<Sense> senses,
        List<Pronunciation> pronunciations,
        Etymology? etymology)
    {
        _senses.Clear();
        _senses.AddRange(senses);
        _pronunciations.Clear();
        _pronunciations.AddRange(pronunciations);
        if (etymology is not null)
        {
            _etymologies.Clear();
            _etymologies.Add(etymology);
        }
        Status = WordStatus.Enriched;
    }

    public void Approve() => Status = WordStatus.Approved;
    public void Archive() => Status = WordStatus.Archived;
    public void MarkFailed() => Status = WordStatus.Failed;
}
