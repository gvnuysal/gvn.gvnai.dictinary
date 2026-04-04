using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetNextQuestion;

public sealed record GetNextQuestionQuery(Guid SessionId) : IQuery<QuizQuestionDto>;
