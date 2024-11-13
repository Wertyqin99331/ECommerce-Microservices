using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Core.Errors;

public record ApplicationError(int StatusCode, string Detail)
{
    public ProblemDetails ToProblemDetails(string title = "Server error")
    {
        return new ProblemDetails()
        {
            Status = this.StatusCode,
            Title = title,
            Detail = this.Detail
        };
    }
    
    public static ApplicationError BadRequest(string detail)
    {
        return new ApplicationError((int)HttpStatusCode.BadRequest, detail);
    }
}