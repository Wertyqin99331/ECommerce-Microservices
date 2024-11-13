using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.MediatR.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request);
        
        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        
        if (timer.Elapsed.Seconds > 3)
            logger.LogWarning("[Performance] The request {Request} took {TimeTaken}",
                typeof(TRequest).Name, timer.Elapsed.Seconds);

        logger.LogInformation("[End] Handled {Request} with {Response}",
            typeof(TRequest).Name, typeof(TResponse).Name);

        return response;
    }
}