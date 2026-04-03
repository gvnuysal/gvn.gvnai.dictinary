using FluentValidation;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;

namespace Gvn.GvnAI.Dictionary.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(DictionaryConstants.MaxEmailLength);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .MaximumLength(DictionaryConstants.MaxFullNameLength);
    }
}
