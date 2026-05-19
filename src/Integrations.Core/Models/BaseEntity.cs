namespace Integrations.Core.Models;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
    }

    public int Id { get; set; }
}
