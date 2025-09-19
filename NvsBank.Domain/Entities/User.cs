using Microsoft.AspNetCore.Identity;
using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public UserRole Role { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();
    public string FullName { get; set; } = string.Empty;
    public Guid PersonId { get; set; }
    public Person? Person { get; set; }
}