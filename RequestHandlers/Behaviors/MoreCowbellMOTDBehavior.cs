using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ViewModels.Common;
using ViewModels.Features.Startup;

namespace RequestHandlers.Behaviors
{
    public class MoreCowbellMOTDBehavior : IPipelineBehavior<WrappedRequest<MOTD.Request, MOTD.Response>, WrappedResponse<MOTD.Response>>
    {
        public async Task<WrappedResponse<MOTD.Response>> Handle(WrappedRequest<MOTD.Request, MOTD.Response> request, CancellationToken cancellationToken, RequestHandlerDelegate<WrappedResponse<MOTD.Response>> next)
        {
            var result = await next();
            if (!result.IsError)
            {
                result.Response.Message += " And more cowbell.";
            }

            return result;
        }
    }
}
