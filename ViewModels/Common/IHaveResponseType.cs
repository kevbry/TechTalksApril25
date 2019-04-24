using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ViewModels.Common
{
    /// <summary>
    /// Marker interface used to tie requests to their corresponding response type
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IHaveResponseType<out TResponse>
    {

    }
}
