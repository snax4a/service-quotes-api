using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Api.Helpers;
using ServiceQuotes.Application.DTOs.Employee;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Api.Controllers
{
    public class EmployeesController : BaseController<EmployeesController>
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Role.Manager)]
        [HttpGet]
        public async Task<ActionResult<List<GetEmployeeResponse>>> GetEmployees([FromQuery] GetEmployeesFilter filter)
        {
            return Ok(await _employeeService.GetAllEmployees(filter));
        }

        [Authorize(Role.Manager)]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(GetEmployeeResponse), 200)]
        public async Task<ActionResult<GetEmployeeWithSpecializationsResponse>> GetEmployeeById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeById(id);

            if (employee is null) return NotFound();

            return Ok(employee);
        }

        [Authorize(Role.Manager)]
        [HttpPut("{id:guid}")]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<GetEmployeeResponse>> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeRequest dto)
        {
            var employee = await _employeeService.GetEmployeeById(id);

            if (employee is null) return NotFound();

            await _employeeService.UpdateEmployee(id, dto);

            return NoContent();
        }

        [Authorize(Role.Manager)]
        [HttpPost("{employeeId:guid}/specializations")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<GetEmployeeResponse>> AssignSpecialization(Guid employeeId, [FromBody] AssignSpecializationRequest dto)
        {
            await _employeeService.AddSpecialization(employeeId, dto.SpecializationId);
            return NoContent();
        }

        [Authorize(Role.Manager)]
        [HttpDelete("{employeeId:guid}/specializations/{specializationId:guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public ActionResult<GetEmployeeResponse> RemoveSpecialization(Guid employeeId, Guid specializationId)
        {
            _employeeService.RemoveSpecialization(employeeId, specializationId);
            return NoContent();
        }
    }
}
