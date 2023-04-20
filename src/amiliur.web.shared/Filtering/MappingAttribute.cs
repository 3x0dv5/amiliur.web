namespace amiliur.web.shared.Filtering;

[AttributeUsage(AttributeTargets.Property)]
public class MappingAttribute : Attribute
{
    public string[] MappingTo { get; set; } = { };
    public string[] MappingFrom { get; set; } = { };
}