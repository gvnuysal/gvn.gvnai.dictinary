using Gvn.GvnFramework.Domain.Aggregates;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class User : AggregateRoot
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string Role { get; private set; } = "User";

    private User() { }

    public static User Create(string email, string passwordHash, string fullName)
    {
        return new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName
        };
    }

    public void UpdateProfile(string fullName)
    {
        FullName = fullName;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}
