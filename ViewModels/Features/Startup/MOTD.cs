using System;
using System.Collections.Generic;
using System.Text;
using ViewModels.Common;

namespace ViewModels.Features.Startup
{
    public static class MOTD
    {
        public class Request : IHaveResponseType<Response>
        {
            /// <summary>
            /// What type of message should be fetched
            /// </summary>
            public enum MessageType
            {
                Joke,
                Quote
            }

            public MessageType Type { get; set; }
        }

        public class Response
        {
            public string Message { get; set; }
        }
    }
}
