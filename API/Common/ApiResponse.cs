// Ignore Spelling: Api

using Application.Common;

namespace API.Common;

public class ApiResponse
{
    public bool Success { get; set; }
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public IEnumerable<ResultError>? Errors { get; set; }
}

public class ApiResponse<TData> : ApiResponse
{
    public TData? Data { get; set; }
}
