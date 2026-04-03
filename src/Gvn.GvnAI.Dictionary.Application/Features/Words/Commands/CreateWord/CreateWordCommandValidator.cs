using FluentValidation;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWord;

public sealed class CreateWordCommandValidator : AbstractValidator<CreateWordCommand>
{
    public CreateWordCommandValidator()
    {
        RuleFor(x => x.Lemma)
            .NotEmpty()
            .MaximumLength(DictionaryConstants.MaxLemmaLength);

        RuleFor(x => x.LanguageId)
            .NotEmpty();

        RuleFor(x => x.PartOfSpeechId)
            .NotEmpty();
    }
}
