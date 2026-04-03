using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Domain.Shared.Errors;

public static class DomainErrors
{
    public static class Word
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("WORD_NOT_FOUND", $"Word with id '{id}' was not found.");

        public static Error NotFoundByLemma(string lemma) =>
            Error.NotFound("WORD_NOT_FOUND", $"Word '{lemma}' was not found.");

        public static Error DuplicateLemma(string lemma) =>
            Error.Conflict("WORD_DUPLICATE", $"Word '{lemma}' already exists in this language.");

        public static Error EnrichmentFailed(string lemma) =>
            Error.Failure("WORD_ENRICHMENT_FAILED", $"AI enrichment failed for word '{lemma}'.");
    }

    public static class Sense
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("SENSE_NOT_FOUND", $"Sense with id '{id}' was not found.");

        public static Error NotInWord(Guid senseId, Guid wordId) =>
            Error.Validation("SENSE_NOT_IN_WORD", $"Sense '{senseId}' does not belong to word '{wordId}'.");
    }

    public static class Translation
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("TRANSLATION_NOT_FOUND", $"Translation with id '{id}' was not found.");
    }

    public static class Example
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("EXAMPLE_NOT_FOUND", $"Example with id '{id}' was not found.");
    }

    public static class Language
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("LANGUAGE_NOT_FOUND", $"Language with id '{id}' was not found.");

        public static Error DuplicateCode(string code) =>
            Error.Conflict("LANGUAGE_DUPLICATE", $"Language with code '{code}' already exists.");
    }

    public static class PartOfSpeech
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("POS_NOT_FOUND", $"Part of speech with id '{id}' was not found.");
    }

    public static class User
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("USER_NOT_FOUND", $"User with id '{id}' was not found.");

        public static Error EmailAlreadyExists(string email) =>
            Error.Conflict("USER_EMAIL_EXISTS", $"A user with email '{email}' already exists.");

        public static readonly Error InvalidCredentials =
            Error.Unauthorized("USER_INVALID_CREDENTIALS", "Invalid email or password.");
    }
}
