using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Example : Entity
{
    public Guid SenseId { get; private set; }
    public Guid? TranslationId { get; private set; }
    public string SourceText { get; private set; } = null!;
    public string? TargetText { get; private set; }
    public ExampleSource Source { get; private set; }

    private Example() { }

    public static Example Create(
        Guid senseId, Guid? translationId,
        string sourceText, string? targetText, ExampleSource source)
    {
        return new Example
        {
            SenseId = senseId,
            TranslationId = translationId,
            SourceText = sourceText,
            TargetText = targetText,
            Source = source
        };
    }

    public void Update(string sourceText, string? targetText, ExampleSource source)
    {
        SourceText = sourceText;
        TargetText = targetText;
        Source = source;
    }
}
