using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetLeaderboard;

public sealed record GetLeaderboardQuery(int Top = 10) : IQuery<List<LeaderboardEntryDto>>;
