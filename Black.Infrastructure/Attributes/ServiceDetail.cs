namespace Black.Infrastructure.Attributes;

public sealed class ServiceDetail : Attribute
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NameWithPrefix { get; set; }
}