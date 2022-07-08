using Black.Domain.Enumeration;

namespace Black.Domain.Base;

public class EntityBase
{
    public virtual long Id { get; set; }
    public virtual Guid GuidId { get; set; }
    public virtual Status Status { get; set; } 
    public virtual string Name { get; set; } 
    public virtual bool Deleted { get; set; } 
    public virtual DateTime CreatedDate { get; set; } = DateTime.Now;
    public virtual DateTime ModifiedDate { get; set; }
    public virtual string CreatedByName { get; set; } 
    public virtual string ModifiedByName { get; set; } 
}

