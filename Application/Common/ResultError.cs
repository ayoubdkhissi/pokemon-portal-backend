namespace Application.Common;
public class ResultError
{
    public string Code { get; set; } = string.Empty;
    public string? ErrorDetails { get; set; }
    public Exception? Exception { get; set; }
    public bool IsExceptionError => Exception is not null;
}