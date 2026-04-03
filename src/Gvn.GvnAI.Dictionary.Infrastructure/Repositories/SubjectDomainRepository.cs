using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class SubjectDomainRepository(DictionaryDbContext context)
    : EfRepository<SubjectDomain, DictionaryDbContext>(context), ISubjectDomainRepository
{
    public async Task<SubjectDomain?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(d => d.Code == code.ToLowerInvariant(), cancellationToken);
    }
}
