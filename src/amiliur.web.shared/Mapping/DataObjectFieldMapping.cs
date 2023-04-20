namespace amiliur.web.shared.Mapping;

public class DataObjectFieldMapping
{
    public string Source { get; set; }
    public string Target { get; set; }

    public DataObjectFieldMapping()
    {
    }

    public DataObjectFieldMapping(string source, string target)
    {
        Source = source;
        Target = target;
    }
}