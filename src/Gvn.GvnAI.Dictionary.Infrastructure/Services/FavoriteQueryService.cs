using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Services;

public class FavoriteQueryService(DictionaryDbContext dbContext) : IFavoriteQueryService
{
    public async Task<List<FavoriteWordDto>> GetFavoriteWordsAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Favorites
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.AddedAt)
            .Join(dbContext.Words, f => f.WordId, w => w.Id, (f, w) => new { f, w })
            .Join(dbContext.Languages, x => x.w.LanguageId, l => l.Id, (x, l) => new { x.f, x.w, Lang = l })
            .Join(dbContext.PartsOfSpeech, x => x.w.PartOfSpeechId, p => p.Id, (x, p) => new { x.f, x.w, x.Lang, Pos = p })
            .Select(x => new FavoriteWordDto(
                x.w.Id,
                x.w.Lemma,
                new LanguageSummaryDto(x.Lang.Id, x.Lang.Code, x.Lang.Name),
                new PartOfSpeechSummaryDto(x.Pos.Id, x.Pos.Code, x.Pos.Name),
                x.w.Senses
                    .OrderBy(s => s.SenseNumber)
                    .SelectMany(s => s.Translations)
                    .Select(t => t.TranslationText)
                    .FirstOrDefault(),
                x.f.AddedAt))
            .ToListAsync(ct);
    }
}
