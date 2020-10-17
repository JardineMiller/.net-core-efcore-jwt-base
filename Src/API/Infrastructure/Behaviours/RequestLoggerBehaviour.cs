using System.Threading;
using System.Threading.Tasks;
using API.Infrastructure.Services;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace API.Infrastructure.Behaviours
{
    public class RequestLoggerBehaviour<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<TRequest> logger;

        public RequestLoggerBehaviour(ILogger<TRequest> logger, ICurrentUserService currentUserService)
        {
            this.logger = logger;
            this.currentUserService = currentUserService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            var serializeObject = JsonConvert.SerializeObject(request);

            this.logger.LogInformation(
                $"Executing Request: [{name}] UserId: [{this.currentUserService.GetId()}] Request Body: [{serializeObject}]");

            return Task.CompletedTask;
        }
    }
}
