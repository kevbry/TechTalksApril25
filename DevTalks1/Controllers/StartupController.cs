using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Common;
using ViewModels.Features.Startup;
using ProblemDetails = ViewModels.Common.ProblemDetails;

namespace DevTalks1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StartupController : ControllerBase
    {
        private IMediator Mediator { get; }

        public StartupController(IMediator mediator)
        {
            this.Mediator = mediator;
        }

        /// <summary>
        /// Fetches either a joke or a quote, depending on the requested message type
        /// </summary>
        /// <param name="request">Identifies the requested message type, either joke or quote</param>
        /// <returns>A message of the requested type</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MOTD.Response>> FetchMOTD(MOTD.Request request)
        {
            var wrapped = new WrappedRequest<MOTD.Request, MOTD.Response>(request);
            var result = await this.Mediator.Send(wrapped);
            if (result.IsError)
            {
                return this.StatusCode(result.Error.Status, result.Error);
            }
            return result.Response;
        }

        /// <summary>
        /// Retrieves a message of the day of the specified type
        /// </summary>
        /// <param name="type">Either a joke, or a quote</param>
        /// <returns>The text of the joke or quote</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MOTD.Response>> MOTD(MOTD.Request.MessageType type)
        {
            var wrapped = new WrappedRequest<MOTD.Request, MOTD.Response>(
                new MOTD.Request()
                {
                    Type = type
                }
            );
            var result = await this.Mediator.Send(wrapped);
            if (result.IsError)
            {
                return this.StatusCode(result.Error.Status, result.Error);
            }
            return result.Response;
        }
    }
}