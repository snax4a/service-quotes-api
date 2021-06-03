using ServiceQuotes.Application.DTOs.Employee;
using ServiceQuotes.Application.Filters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Interfaces
{
    public interface IEmployeeService : IDisposable
    {
        Task<List<GetEmployeeWithSpecializationsResponse>> GetAllEmployees(GetEmployeesFilter filter);

        Task<GetEmployeeWithSpecializationsResponse> GetEmployeeById(Guid id);

        Task<GetEmployeeResponse> CreateEmployee(CreateEmployeeRequest dto);

        Task<GetEmployeeResponse> UpdateEmployee(Guid id, UpdateEmployeeRequest dto);

        Task<GetEmployeeWithSpecializationsResponse> AddSpecialization(Guid employeeId, Guid specializationId);
        void RemoveSpecialization(Guid employeeId, Guid specializationId);
    }
}
