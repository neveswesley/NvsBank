using NvsBank.Domain.Entities.Enums;

namespace NvsBank.Domain.Entities;

public class PixArea : BaseEntity
{
    public Guid AccountId { get; set; }
    public PixKeyType KeyType { get; set; }
    public string KeyValue { get; set; }
    public PixKeyStatus Status { get; set; } = PixKeyStatus.Active;
    
    public Account Account { get; set; }

    public PixArea()
    {
        
    }
}