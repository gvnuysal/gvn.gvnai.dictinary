using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetQuizResult;

public sealed record GetQuizResultQuery(Guid SessionId) : IQuery<QuizResultDto>;
