using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Services;

public interface IResultHandler
{
    public IActionResult HandleResult(OperationResult result);
    public IActionResult HandleResult<TData>(OperationResult<TData> result);
}