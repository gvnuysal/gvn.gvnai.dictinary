using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class WordRepository(DictionaryDbContext context)
    : EfRepository<Word, DictionaryDbContext>(context), IWordRepository
{
    public async Task<Word?> GetByLemmaAsync(string lemma, Guid languageId, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var queryable = DbSet.Where(w => w.Lemma == lemma && w.LanguageId == languageId);

        if (userId.HasValue)
            queryable = queryable.Where(w => w.UserId == userId.Value);

        return await queryable.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Word?> GetByIdWithSensesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(w => w.Senses)
                .ThenInclude(s => s.Translations)
            .Include(w => w.Senses)
                .ThenInclude(s => s.Examples)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<Word?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(w => w.Senses)
                .ThenInclude(s => s.Translations)
            .Include(w => w.Senses)
                .ThenInclude(s => s.Examples)
            .Include(w => w.Pronunciations)
            .Include(w => w.Etymologies)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<Word> Items, int TotalCount)> SearchAsync(
        string? query, Guid? languageId, Guid? partOfSpeechId, Guid? domainId, Guid? registerId,
        int skip, int take, Guid? userId = null, CancellationToken cancellationToken = default)
    {
        var queryable = DbSet
            .Include(w => w.Senses)
            .AsQueryable();

        if (userId.HasValue)
            queryable = queryable.Where(w => w.UserId == userId.Value);

        if (!string.IsNullOrWhiteSpace(query))
            queryable = queryable.Where(w => EF.Functions.ILike(w.Lemma, $"%{query}%"));

        if (languageId.HasValue)
            queryable = queryable.Where(w => w.LanguageId == languageId.Value);

        if (partOfSpeechId.HasValue)
            queryable = queryable.Where(w => w.PartOfSpeechId == partOfSpeechId.Value);

        if (domainId.HasValue)
            queryable = queryable.Where(w => w.Senses.Any(s => s.DomainId == domainId.Value));

        if (registerId.HasValue)
            queryable = queryable.Where(w => w.Senses.Any(s => s.RegisterId == registerId.Value));

        var totalCount = await queryable.CountAsync(cancellationToken);
        var items = await queryable
            .OrderBy(w => w.Lemma)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IEnumerable<Word>> GetPendingWordsAsync(int count, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(w => w.Status == WordStatus.Pending)
            .OrderBy(w => w.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
