namespace CoP.Demo.EF.Domain;

public class AuditLog
{
    public Guid Id { get; set; }
    public string Action { get; set; }
    
    public DateTime CreatedAt { get; set; }


    public static AuditLog Create(string action)
        => new() { Id = Guid.NewGuid(), Action = action, CreatedAt = DateTime.UtcNow };
}