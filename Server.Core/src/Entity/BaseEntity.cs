namespace Server.Core.src.Entity;

public class BaseEntity : Timestamp
{
    public Guid Id { get; set; } = Guid.NewGuid();
}