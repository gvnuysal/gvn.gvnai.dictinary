using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Domain.Repositories;

public interface IWordRepository : IRepository<Word>
{
    Task<Word?> GetByLemmaAsync(string lemma, Guid languageId, Guid? userId = null, CancellationToken cancellationToken = default);
    Task<Word?> GetByIdWithSensesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Word?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Word> Items, int TotalCount)> SearchAsync(
        string? query, Guid? languageId, Guid? partOfSpeechId, Guid? domainId, Guid? registerId,
        int skip, int take, Guid? userId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Word>> GetPendingWordsAsync(int count, CancellationToken cancellationToken = default);
}
