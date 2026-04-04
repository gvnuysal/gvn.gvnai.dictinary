using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.SubmitAnswer;

public sealed record SubmitAnswerCommand(
    Guid SessionId,
    Guid WordId,
    Guid? SelectedOptionId,  // null = timeout
    Guid CorrectOptionId,
    int ResponseTimeMs) : ICommand<QuizAnswerResultDto>;
