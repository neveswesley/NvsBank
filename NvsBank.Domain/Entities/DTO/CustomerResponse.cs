using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class CustomerResponse(
    Guid Id,
    string FullName,
    CustomerType type,
    string documentNumber,
    DateTime? birthDate,
    DateTime? foundationDate,
    string phoneNumber,
    string email,
    CustomerStatus status
);