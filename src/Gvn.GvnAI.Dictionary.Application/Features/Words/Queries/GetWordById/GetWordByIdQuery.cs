using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWordById;

public sealed record GetWordByIdQuery(Guid Id) : IQuery<WordDto>;
