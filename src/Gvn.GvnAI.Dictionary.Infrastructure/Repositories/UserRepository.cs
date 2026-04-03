using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class UserRepository(DictionaryDbContext context)
    : EfRepository<User, DictionaryDbContext>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), cancellationToken);
    }
}
