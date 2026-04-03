using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class RegisterRepository(DictionaryDbContext context)
    : EfRepository<Register, DictionaryDbContext>(context), IRegisterRepository
{
    public async Task<Register?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(r => r.Code == code.ToLowerInvariant(), cancellationToken);
    }
}
