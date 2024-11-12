using System.Net;

namespace Core.Errors;

public record ApplicationError(int StatusCode, string Detail)
{
    public static ApplicationError BadRequest(string detail)
    {
        return new ApplicationError((int)HttpStatusCode.BadRequest, detail);
    }
}