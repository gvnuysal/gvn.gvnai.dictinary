using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Profile.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(Guid UserId, string FullName) : ICommand;
