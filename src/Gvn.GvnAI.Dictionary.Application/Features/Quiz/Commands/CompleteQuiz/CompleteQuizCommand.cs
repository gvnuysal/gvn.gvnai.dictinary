using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.CompleteQuiz;

public sealed record CompleteQuizCommand(Guid SessionId) : ICommand<QuizResultDto>;
