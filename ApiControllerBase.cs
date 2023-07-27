using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenantApi.Api.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;

        protected ApiControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }

        /// <summary>
        /// Send a request without specified result model.
        /// </summary>
        /// <param name="request">Command or query model</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns></returns>
        protected async Task<IActionResult> Send(IRequest<Result> request, CancellationToken ct = default)
        {
            var commandResult = await Mediator.Send(request, ct);
            return Ok(commandResult);
        }

        /// <summary>
        /// Send a request with the specified result model.
        /// </summary>
        /// <param name="request">Command or query model</param>
        /// <param name="ct">Cancellation token</param>
        protected async Task<IActionResult> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            var commandResult = await Mediator.Send(request, ct);
            return Ok(commandResult);
        }
    }
}
