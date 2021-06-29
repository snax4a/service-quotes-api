using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Specialization;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class SpecializationsController : BaseController<SpecializationsController>
    {
        private readonly ISpecializationService _specializationService;
        private readonly ILogger _logger;

        public SpecializationsController(
            ISpecializationService specializationService,
            ILogger<SpecializationsController> logger)
        {
            _specializationService = specializationService;
            _logger = logger;
        }

        [Authorize(Role.Manager)]
        [HttpGet]
        public async Task<ActionResult<List<GetSpecializationResponse>>> GetSpecializations([FromQuery] GetSpecializationsFilter filter)
        {
            return Ok(await _specializationService.GetAllSpecializations(filter));
        }

        [Authorize(Role.Manager)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetSpecializationResponse), 200)]
        public async Task<ActionResult<GetSpecializationResponse>> GetSpecializationById(Guid id)
        {
            var specialization = await _specializationService.GetSpecializationById(id);

            if (specialization is null) return NotFound();

            return Ok(specialization);
        }

        [Authorize(Role.Manager)]
        [HttpPut("{specializationId:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetSpecializationResponse>> UpdateSpecialization(Guid specializationId, [FromBody] UpdateSpecializationRequest dto)
        {
            var specialization = await _specializationService.GetSpecializationById(specializationId);

            if (specialization is null) return NotFound();

            await _specializationService.UpdateSpecialization(specializationId, dto);

            return NoContent();
        }

        [Authorize(Role.Manager)]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<GetSpecializationResponse>> CreateSpecialization(CreateSpecializationRequest dto)
        {
            var specialization = await _specializationService.CreateSpecialization(dto);
            return CreatedAtAction("GetSpecializationById", new { id = specialization.Id }, specialization);
        }

        [Authorize(Role.Manager)]
        [HttpDelete("{specializationId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> RemoveSpecialization(Guid specializationId)
        {
            Console.WriteLine("### Value: " + specializationId);
            await _specializationService.RemoveSpecialization(specializationId);
            return NoContent();
        }
    }
}
