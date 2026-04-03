using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.UpdateWord;

public sealed record UpdateWordCommand(
    Guid Id,
    string Lemma,
    Guid LanguageId,
    Guid PartOfSpeechId,
    int? FrequencyRank,
    DifficultyLevel? DifficultyLevel,
    bool IsCompound,
    bool IsIdiom,
    bool IsProperNoun) : ICommand;
