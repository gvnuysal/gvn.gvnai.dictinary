using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnAI.Dictionary.Infrastructure.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Services;
using Gvn.GvnFramework.Domain.Repositories;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Gvn.GvnFramework.EntityFramewokCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gvn.GvnAI.Dictionary.Infrastructure.DependencyInjection;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddDictionaryInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<DictionaryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<ISenseRepository, SenseRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IPartOfSpeechRepository, PartOfSpeechRepository>();
        services.AddScoped<IRegisterRepository, RegisterRepository>();
        services.AddScoped<ISubjectDomainRepository, SubjectDomainRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepository<Translation>, EfRepository<Translation, DictionaryDbContext>>();
        services.AddScoped<IRepository<Example>, EfRepository<Example, DictionaryDbContext>>();
        services.AddScoped<IRepository<Pronunciation>, EfRepository<Pronunciation, DictionaryDbContext>>();
        services.AddScoped<IRepository<Etymology>, EfRepository<Etymology, DictionaryDbContext>>();

        // Quiz
        services.AddScoped<IRepository<QuizSession>, EfRepository<QuizSession, DictionaryDbContext>>();
        services.AddScoped<IRepository<QuizAnswer>, EfRepository<QuizAnswer, DictionaryDbContext>>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork<DictionaryDbContext>>();

        // AI Service
        services.AddScoped<IAiDictionaryService, ClaudeAiDictionaryService>();

        // Favorites
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IFavoriteQueryService, FavoriteQueryService>();

        // Profile
        services.AddScoped<IProfileQueryService, ProfileQueryService>();

        // Quiz Service
        services.AddScoped<IQuizService, QuizService>();

        return services;
    }
}
