using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Profile.Queries.GetProfile;

public sealed record GetProfileQuery(Guid UserId) : IQuery<ProfileDto>;
