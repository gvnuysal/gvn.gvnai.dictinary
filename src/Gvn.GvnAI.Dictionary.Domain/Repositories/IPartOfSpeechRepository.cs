using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Domain.Repositories;

public interface IPartOfSpeechRepository : IRepository<PartOfSpeech>
{
    Task<PartOfSpeech?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
