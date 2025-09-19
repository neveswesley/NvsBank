using NvsBank.Domain.Entities;

namespace NvsBank.Domain.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}