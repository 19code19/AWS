using Microsoft.AspNetCore.Mvc;
using MultiTenantApi.Api.Commands.Sample.Create;
using MultiTenantApi.Api.Commands.Sample.Update;
using MultiTenantApi.Api.Commands.Sample.Delete;
using MultiTenantApi.Api.Queries.Sample.ListQuery;
using MultiTenantApi.Api.Queries.Sample.IdQuery;
using System.Threading.Tasks;
using MediatR;
using MultiTenantApi.Api.Models.Sample;

namespace MultiTenantApi.Api.Controllers
{
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Route("{tenant}/v{version:apiVersion}/Sample")]
    public class SampleController : ApiControllerBase
    {
        public SampleController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Get all Sample.
        /// </summary>
        /// <response code="200">List of Sample returned.</response>
        /// <response code="401">The user not authenticated.</response>
        /// <response code="403">The user not authorised.</response>
        /// <response code="500">System error.</response>
        [HttpGet(Name = "GetSampleList")]
        [ProducesResponseType(200, Type = typeof(ApiListResult<SampleModel>))]
        [ProducesResponseType(401, Type = typeof(ErrorResult))]
        [ProducesResponseType(403, Type = typeof(ErrorResult))]
        [ProducesResponseType(500, Type = typeof(ErrorResult))]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetSampleListAsync() =>
            await Send(new GetSampleListQueryRequest());

        /// <summary>
        /// Get a single Sample by id
        /// </summary>
        /// <response code="200">Single Sample returned.</response>
        /// <response code="401">The user not authenticated.</response>
        /// <response code="403">The user not authorised.</response>
        /// <response code="500">System error.</response>
        [HttpGet("{id}", Name = "GetSample")]
        [ProducesResponseType(200, Type = typeof(SampleModel))]
        [ProducesResponseType(401, Type = typeof(ErrorResult))]
        [ProducesResponseType(403, Type = typeof(ErrorResult))]
        [ProducesResponseType(500, Type = typeof(ErrorResult))]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public async Task<IActionResult> GetSampleByIdAsync(string id) =>
            await Send(new GetSampleByIdQueryRequest(id));

        /// <summary>
        /// Creates a new Sample object
        /// </summary>
        /// <response code="201">Sample object created.</response>
        /// <response code="401">The user not authenticated.</response>
        /// <response code="403">The user not authorised.</response>
        /// <response code="500">System error.</response>
        [HttpPost(Name = "CreateSample")]
        [ProducesResponseType(201, Type = typeof(CreateSampleCommandResponse))]
        [ProducesResponseType(401, Type = typeof(ErrorResult))]
        [ProducesResponseType(403, Type = typeof(ErrorResult))]
        [ProducesResponseType(500, Type = typeof(ErrorResult))]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<IActionResult> CreateSampleAsync([FromBody] CreateSampleCommandRequest command) =>
            await Send(command);

        /// <summary>
        /// Updates a Sample object
        /// </summary>
        /// <response code="200">Sample object updated.</response>
        /// <response code="401">The user not authenticated.</response>
        /// <response code="403">The user not authorised.</response>
        /// <response code="500">System error.</response>
        [HttpPut("{id}", Name = "UpdateSample")]
        [ProducesResponseType(200, Type = typeof(UpdateSampleCommandResponse))]
        [ProducesResponseType(401, Type = typeof(ErrorResult))]
        [ProducesResponseType(403, Type = typeof(ErrorResult))]
        [ProducesResponseType(500, Type = typeof(ErrorResult))]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<IActionResult> UpdateSampleAsync(string id, [FromBody] UpdateSampleCommandRequest command) =>
            await Send(command);

        /// <summary>
        /// Deletes a Sample object
        /// </summary>
        /// <response code="204">Sample object deleted.</response>
        /// <response code="401">The user not authenticated.</response>
        /// <response code="403">The user not authorised.</response>
        /// <response code="500">System error.</response>
        [HttpDelete("{id}", Name = "DeleteSample")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401, Type = typeof(ErrorResult))]
        [ProducesResponseType(403, Type = typeof(ErrorResult))]
        [ProducesResponseType(500, Type = typeof(ErrorResult))]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async Task<IActionResult> DeleteSampleAsync(string id) =>
            await Send(new DeleteSampleCommandRequest(id));
    }
}
