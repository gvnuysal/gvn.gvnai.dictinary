using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.SubmitAnswer;

public sealed class SubmitAnswerCommandHandler(
    IRepository<QuizSession> quizSessionRepository,
    IRepository<QuizAnswer> quizAnswerRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SubmitAnswerCommand, QuizAnswerResultDto>
{
    public async Task<Result<QuizAnswerResultDto>> Handle(SubmitAnswerCommand request, CancellationToken cancellationToken)
    {
        var session = await quizSessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            return Result<QuizAnswerResultDto>.Fail(Error.NotFound("QuizSession.NotFound", $"Quiz session {request.SessionId} not found."));

        if (session.IsCompleted)
            return Result<QuizAnswerResultDto>.Fail(Error.Validation("QuizSession.Completed", "This quiz session is already completed."));

        var isCorrect = request.SelectedOptionId.HasValue && request.SelectedOptionId == request.CorrectOptionId;
        var pointsEarned = isCorrect ? 5 : -3;

        // Answer'ı doğrudan repository ile ekle (backing field concurrency fix)
        var answer = QuizAnswer.Create(
            request.SessionId, request.WordId, request.SelectedOptionId, request.CorrectOptionId,
            isCorrect, pointsEarned, request.ResponseTimeMs);
        await quizAnswerRepository.AddAsync(answer, cancellationToken);

        // Session'ın skorlarını güncelle
        session.RecordAnswer(isCorrect, pointsEarned);
        await quizSessionRepository.UpdateAsync(session, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<QuizAnswerResultDto>.Ok(new QuizAnswerResultDto(
            isCorrect, pointsEarned, session.TotalScore, request.CorrectOptionId, request.SelectedOptionId));
    }
}
