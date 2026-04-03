using System.Security.Claims;
using Gvn.GvnAI.Dictionary.Application.Features.Auth.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Security.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return Result<LoginResponse>.Fail(DomainErrors.User.InvalidCredentials);

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result<LoginResponse>.Fail(DomainErrors.User.InvalidCredentials);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role)
        };

        var token = tokenService.GenerateToken(claims);
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        return Result<LoginResponse>.Ok(new LoginResponse(token, expiresAt));
    }
}
