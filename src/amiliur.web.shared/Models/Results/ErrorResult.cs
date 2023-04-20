namespace amiliur.web.shared.Models.Results;

public class ErrorResult
{
    public string ErrorMessage { get; set; }
    public ErrorResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}