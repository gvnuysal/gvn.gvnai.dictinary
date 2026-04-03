using FluentValidation;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWordWithTranslation;

public sealed class CreateWordWithTranslationCommandValidator : AbstractValidator<CreateWordWithTranslationCommand>
{
    public CreateWordWithTranslationCommandValidator()
    {
        RuleFor(x => x.Lemma)
            .NotEmpty().WithMessage("English word is required.")
            .MaximumLength(DictionaryConstants.MaxLemmaLength);

        RuleFor(x => x.PartOfSpeechId)
            .NotEmpty().WithMessage("Part of speech is required.");

        RuleFor(x => x.Definition)
            .NotEmpty().WithMessage("Definition is required.")
            .MaximumLength(DictionaryConstants.MaxDefinitionLength);

        RuleFor(x => x.TranslationText)
            .NotEmpty().WithMessage("Turkish translation is required.")
            .MaximumLength(DictionaryConstants.MaxTranslationTextLength);
    }
}
