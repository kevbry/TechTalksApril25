using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ViewModels.Common
{
    public class WrappedRequest<TRequest,TResponse> : IRequest<WrappedResponse<TResponse>> 
        where TRequest : IHaveResponseType<TResponse>
    {
        public TRequest Request { get; set; }

        public WrappedRequest(TRequest request)
        {
            this.Request = request;
        }
    }
}
