using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Domain.Repositories;

public interface IRegisterRepository : IRepository<Register>
{
    Task<Register?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
