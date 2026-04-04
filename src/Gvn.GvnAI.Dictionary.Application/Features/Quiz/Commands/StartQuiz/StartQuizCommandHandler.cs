using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Commands.StartQuiz;

public sealed class StartQuizCommandHandler(
    IRepository<QuizSession> quizSessionRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<StartQuizCommand, Guid>
{
    public async Task<Result<Guid>> Handle(StartQuizCommand request, CancellationToken cancellationToken)
    {
        var session = QuizSession.Create(request.UserId);

        await quizSessionRepository.AddAsync(session, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Ok(session.Id);
    }
}
