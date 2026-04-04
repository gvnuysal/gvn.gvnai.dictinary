using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.StartQuiz;

public sealed record StartQuizCommand(Guid UserId) : ICommand<Guid>;
