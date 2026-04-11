using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Profile.Commands.UpdateApiSettings;

public sealed class UpdateApiSettingsCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateApiSettingsCommand>
{
    public async Task<Result> Handle(UpdateApiSettingsCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Fail(Error.NotFound("USER_NOT_FOUND", "User not found."));

        user.UpdateApiSettings(request.TranslateProvider, request.ClaudeApiKey, request.GoogleTranslateApiKey, request.QuizAutoSpeak);
        await userRepository.UpdateAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
