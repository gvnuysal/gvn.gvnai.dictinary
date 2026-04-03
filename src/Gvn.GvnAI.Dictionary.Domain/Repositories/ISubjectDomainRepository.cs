using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Domain.Repositories;

public interface ISubjectDomainRepository : IRepository<SubjectDomain>
{
    Task<SubjectDomain?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
