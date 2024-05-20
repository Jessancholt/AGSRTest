using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Test.Core.Models;
using Test.Core.Services.Interfaces;

namespace Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsService _patientsService;

        public PatientsController(IPatientsService patientsService)
        {
            _patientsService = patientsService;
        }

        /// <summary>
        /// Gets the list of patients.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, "List of all patients")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<PatientContext>>> Get(CancellationToken cancellationToken)
        {
            var response = await _patientsService.GetAsync(cancellationToken);

            return new ObjectResult(response);
        }

        /// <summary>
        /// Gets the patient by specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Specified patient")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PatientContext>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var response = await _patientsService.GetAsync(id, cancellationToken);

            return new ObjectResult(response);
        }

        /// <summary>
        /// Gets the patients between dates.
        /// </summary>
        /// <param name="from">Date from.</param>
        /// <param name="to">Date to.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("filter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Specified patient")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<PatientContext>>> FindByDate([FromQuery] string date, CancellationToken cancellationToken)
        {
            var response = await _patientsService.GetByDateAsync(date, cancellationToken);

            return new ObjectResult(response);
        }

        /// <summary>
        /// Creates the patient.
        /// </summary>
        /// <param name="model">Create model.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, "Patient created")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PatientContext>> Create([FromBody] PatientCreateModel model, CancellationToken cancellationToken)
        {
            var response = await _patientsService.CreateAsync(model, cancellationToken);

            return new ObjectResult(response);
        }

        /// <summary>
        /// Updates the patient.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="model">Update model.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Patient updated")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PatientContext>> Update([FromRoute] Guid id, [FromBody] PatientEditModel model, CancellationToken cancellationToken)
        {
            var response = await _patientsService.UpdateAsync(id, model, cancellationToken);

            return new ObjectResult(response);
        }

        /// <summary>
        /// Updates the patient.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Patient deleted")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _patientsService.DeleteAsync(id, cancellationToken));
        }
    }
}
