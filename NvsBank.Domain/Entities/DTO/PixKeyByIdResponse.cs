using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities.DTO;

public class PixKeyByIdResponse
{
    public PixKeyType KeyType { get; set; }
    public string KeyValue { get; set; }
    public PixKeyStatus Status { get; set; }
}