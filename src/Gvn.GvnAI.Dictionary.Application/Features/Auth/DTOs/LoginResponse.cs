namespace Gvn.GvnAI.Dictionary.Application.Features.Auth.DTOs;

public sealed record LoginResponse(string Token, DateTime ExpiresAt);
