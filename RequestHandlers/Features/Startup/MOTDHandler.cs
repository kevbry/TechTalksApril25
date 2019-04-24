using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientAbstractions;
using MediatR;
using ViewModels.Common;
using ViewModels.Features.Startup;

namespace RequestHandlers.Features.Startup
{
    public class MOTDHandler : IRequestHandler<WrappedRequest<MOTD.Request, MOTD.Response>, WrappedResponse<MOTD.Response>>
    {
        public MOTDHandler(IJokeClient client)
        {
            this.Client = client;
        }

        public IJokeClient Client { get; set; }

        public async Task<WrappedResponse<MOTD.Response>> Handle(WrappedRequest<MOTD.Request, MOTD.Response> request, CancellationToken cancellationToken)
        {
            if (request.Request.Type == MOTD.Request.MessageType.Joke)
            {
                var joke = await this.Client.GetJoke();
                return new WrappedResponse<MOTD.Response>(new MOTD.Response()
                {
                    Message = joke
                });
            }
            else
            {
                return new WrappedResponse<MOTD.Response>(new MOTD.Response()
                {
                    Message = "I don't always test, but when I do, I do it in production\n --Kevin Bryant"
                });
            }
        }


    }
}
