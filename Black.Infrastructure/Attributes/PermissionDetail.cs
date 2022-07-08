namespace Black.Infrastructure.Attributes;

public sealed class PermissionDetail : Attribute
{
    public string Name { get; set; }
    public string Description { get; set; }
}

