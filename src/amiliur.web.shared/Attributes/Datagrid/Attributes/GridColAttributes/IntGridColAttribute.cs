namespace amiliur.web.shared.Attributes.Datagrid.Attributes.GridColAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class IntGridColAttribute : DecimalGridColAttribute
{
    public IntGridColAttribute()
    {
        DecimalPlaces = 0;
    }
}