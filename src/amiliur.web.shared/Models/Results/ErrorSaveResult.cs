namespace amiliur.web.shared.Models.Results;

public class ErrorSaveResult : SaveBaseResult
{
    public string ModelJson { get; set; } = null!;

    public ErrorSaveResult()
    {
        Success = false;
    }

    public ErrorSaveResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
        Success = false;
    }

    public ErrorSaveResult(Exception e)
    {
        ErrorMessage = e.Message;
        Success = false;
    }
}