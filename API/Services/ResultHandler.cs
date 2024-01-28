using API.Common;
using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Services;

public class ResultHandler : IResultHandler
{
    public IActionResult HandleResult(OperationResult result)
    {
        var apiResponse = new ApiResponse
        {
            Success = result.IsSuccess,
            ErrorCode = result.ErrorCode,
            Errors = result.Errors,
            Message = result.Message

        };
        return StatusCode(result.StatusCode, apiResponse);
    }

    public IActionResult HandleResult<TData>(OperationResult<TData> result)
    {
        var apiResponse = new ApiResponse<TData>
        {
            Success = result.IsSuccess,
            ErrorCode = result.ErrorCode,
            Errors = result.Errors,
            Message = result.Message,
            Data = result.Data
        };
        return StatusCode(result.StatusCode, apiResponse);

    }

    private static ObjectResult StatusCode([ActionResultStatusCode] int statusCode, [ActionResultObjectValue] object value)
        => new (value)
        {
            StatusCode = statusCode
        };
}