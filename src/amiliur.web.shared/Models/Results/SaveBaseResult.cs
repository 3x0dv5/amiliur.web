namespace amiliur.web.shared.Models.Results;

public class SaveBaseResult
{
    public bool Success { get; set; } 
    public string? ErrorMessage { get; set; }
    public string? Id { get; set; }
    public string? Code { get; set; }
    public string? Nome { get; set; }
    public DateTime Ts { get; set; } = DateTime.UtcNow;
}