using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetQuizResult;

public sealed class GetQuizResultQueryHandler(
    IRepository<QuizSession> quizSessionRepository,
    IQuizService quizService) : IQueryHandler<GetQuizResultQuery, QuizResultDto>
{
    public async Task<Result<QuizResultDto>> Handle(GetQuizResultQuery request, CancellationToken cancellationToken)
    {
        var session = await quizSessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            return Result<QuizResultDto>.Fail(Error.NotFound("QuizSession.NotFound", $"Quiz session {request.SessionId} not found."));

        var accuracy = session.TotalQuestions > 0
            ? Math.Round((double)session.CorrectCount / session.TotalQuestions * 100, 2)
            : 0;

        var answerDetails = await quizService.GetAnswerDetailsBySessionAsync(session.Id, cancellationToken);

        return Result<QuizResultDto>.Ok(new QuizResultDto(
            session.Id, session.TotalScore, session.CorrectCount, session.WrongCount,
            session.TotalQuestions, accuracy, answerDetails));
    }
}
