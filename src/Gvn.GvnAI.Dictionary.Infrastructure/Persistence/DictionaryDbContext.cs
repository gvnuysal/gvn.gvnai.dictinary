using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.EntityFramewokCore.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence;

public class DictionaryDbContext(
    DbContextOptions<DictionaryDbContext> options,
    IMediator? mediator = null) : GvnDbContext<DictionaryDbContext>(options, mediator)
{
    public DbSet<Word> Words => Set<Word>();
    public DbSet<Sense> Senses => Set<Sense>();
    public DbSet<Translation> Translations => Set<Translation>();
    public DbSet<Example> Examples => Set<Example>();
    public DbSet<Pronunciation> Pronunciations => Set<Pronunciation>();
    public DbSet<Etymology> Etymologies => Set<Etymology>();
    public DbSet<SenseSynonym> SenseSynonyms => Set<SenseSynonym>();
    public DbSet<SenseAntonym> SenseAntonyms => Set<SenseAntonym>();
    public DbSet<WordRelationship> WordRelationships => Set<WordRelationship>();
    public DbSet<Language> Languages => Set<Language>();
    public DbSet<PartOfSpeech> PartsOfSpeech => Set<PartOfSpeech>();
    public DbSet<Register> Registers => Set<Register>();
    public DbSet<SubjectDomain> SubjectDomains => Set<SubjectDomain>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DictionaryDbContext).Assembly);
    }
}
