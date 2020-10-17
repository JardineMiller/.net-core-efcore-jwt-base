using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace API.Infrastructure.Behaviours
{
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<TRequest> logger;
        private readonly Stopwatch timer;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            this.timer = new Stopwatch();
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            this.timer.Start();

            var response = await next();

            this.timer.Stop();

            if (this.timer.ElapsedMilliseconds > 500)
            {
                var name = typeof(TRequest).Name;

                this.logger.LogWarning(
                    $"Long Running Request: [{name}] [{this.timer.ElapsedMilliseconds}] [{this.currentUserService.GetId()}] [{request}]");
            }

            return response;
        }
    }
}
