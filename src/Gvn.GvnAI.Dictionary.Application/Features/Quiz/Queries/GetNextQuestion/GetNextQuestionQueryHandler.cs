using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetNextQuestion;

public sealed class GetNextQuestionQueryHandler(
    IRepository<QuizSession> quizSessionRepository,
    IQuizService quizService) : IQueryHandler<GetNextQuestionQuery, QuizQuestionDto>
{
    public async Task<Result<QuizQuestionDto>> Handle(GetNextQuestionQuery request, CancellationToken cancellationToken)
    {
        var session = await quizSessionRepository.GetByIdAsync(request.SessionId, cancellationToken);
        if (session is null)
            return Result<QuizQuestionDto>.Fail(Error.NotFound("QuizSession.NotFound", $"Quiz session {request.SessionId} not found."));

        if (session.IsCompleted)
            return Result<QuizQuestionDto>.Fail(Error.Validation("QuizSession.Completed", "This quiz session is already completed."));

        var askedWordIds = await quizService.GetAskedWordIdsAsync(request.SessionId, cancellationToken);

        // Kullanıcının kendi kelimelerinden soru getir
        var wordData = await quizService.GetRandomWordWithTranslationAsync(session.UserId, askedWordIds, cancellationToken);
        if (wordData is null)
            return Result<QuizQuestionDto>.Fail(Error.NotFound("Quiz.NoMoreWords", "No more words available for the quiz."));

        var (wordId, lemma, definition, correctTranslationId, correctTranslationText) = wordData.Value;

        // Yanlış seçenekler de kullanıcının kelimelerinden
        var wrongOptions = await quizService.GetRandomWrongOptionsAsync(session.UserId, wordId, 4, cancellationToken);

        var options = new List<QuizOptionDto> { new(correctTranslationId, correctTranslationText) };
        options.AddRange(wrongOptions.Select(o => new QuizOptionDto(o.TranslationId, o.Text)));

        var random = new Random();
        options = options.OrderBy(_ => random.Next()).ToList();

        return Result<QuizQuestionDto>.Ok(new QuizQuestionDto(
            session.Id, wordId, lemma, definition, options, correctTranslationId,
            session.TotalQuestions + 1, session.TotalScore, session.CorrectCount, session.WrongCount));
    }
}
