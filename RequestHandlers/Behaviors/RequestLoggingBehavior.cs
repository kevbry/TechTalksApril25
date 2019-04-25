using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ViewModels.Common;
using ViewModels.Features.Startup;

namespace RequestHandlers.Behaviors
{
    public class RequestLoggingBehavior : IPipelineBehavior<WrappedRequest<MOTD.Request, MOTD.Response>, WrappedResponse<MOTD.Response>>
    {
        public RequestLoggingBehavior(ILogger<RequestLoggingBehavior> logger)
        {
            this.Logger = logger;
        }

        public ILogger<RequestLoggingBehavior> Logger { get; set; }

        public async Task<WrappedResponse<MOTD.Response>> Handle(WrappedRequest<MOTD.Request, MOTD.Response> request, CancellationToken cancellationToken, RequestHandlerDelegate<WrappedResponse<MOTD.Response>> next)
        {
            Logger.LogDebug("Request: {request}", JsonConvert.SerializeObject(request));
            return await next();
        }
    }
}
