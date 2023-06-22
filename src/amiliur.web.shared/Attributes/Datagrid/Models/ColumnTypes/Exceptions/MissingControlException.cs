namespace amiliur.web.shared.Attributes.Datagrid.Models.ColumnTypes.Exceptions;

public class MissingControlException : Exception
{
    public MissingControlException(string controlName): base(string.Format(ExceptionMessages.MissingControlExceptionMessageFormat, controlName))
    {
    }
}