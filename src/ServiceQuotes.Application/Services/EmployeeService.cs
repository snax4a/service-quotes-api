using AutoMapper;
using LinqKit;
using ServiceQuotes.Application.DTOs.Employee;
using ServiceQuotes.Application.DTOs.Specialization;
using ServiceQuotes.Application.Exceptions;
using ServiceQuotes.Application.Filters;
using ServiceQuotes.Application.Interfaces;
using ServiceQuotes.Domain;
using ServiceQuotes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceQuotes.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetEmployeeWithSpecializationsResponse>> GetAllEmployees(GetEmployeesFilter filter)
        {
            // prepare filter predicate
            var predicate = PredicateBuilder.New<Employee>(true);

            if (!string.IsNullOrEmpty(filter?.SearchString))
            {
                predicate = predicate.Or(p => p.FirstName.ToLower().Contains(filter.SearchString.ToLower()));
                predicate = predicate.Or(p => p.LastName.ToLower().Contains(filter.SearchString.ToLower()));
            }

            var employees = await _unitOfWork.Employees.FindWithSpecializations(predicate);

            return employees.Select(employee => new GetEmployeeWithSpecializationsResponse()
            {
                Id = employee.Id,
                AccountId = employee.AccountId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Specializations = employee.EmployeeSpecializations
                    .Select(es => new GetSpecializationResponse()
                    {
                        Id = es.Specialization.Id,
                        Name = es.Specialization.Name
                    })
                    .ToList()
            })
            .ToList();
        }

        public async Task<GetEmployeeWithSpecializationsResponse> GetEmployeeById(Guid id)
        {
            var employee = await _unitOfWork.Employees.GetWithSpecializations(id);

            return new GetEmployeeWithSpecializationsResponse()
            {
                Id = employee.Id,
                AccountId = employee.AccountId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Specializations = employee.EmployeeSpecializations
                    .Select(es => new GetSpecializationResponse()
                    {
                        Id = es.Specialization.Id,
                        Name = es.Specialization.Name
                    })
                    .ToList()
            };
        }

        public async Task<GetEmployeeResponse> CreateEmployee(CreateEmployeeRequest dto)
        {
            // validate
            if (await _unitOfWork.Accounts.Get(dto.AccountId) is null)
                throw new KeyNotFoundException("Account does not exist.");

            // map dto to new employee object
            var newEmployee = _mapper.Map<Employee>(dto);

            _unitOfWork.Employees.Add(newEmployee);
            _unitOfWork.Commit();

            return _mapper.Map<GetEmployeeResponse>(newEmployee);
        }

        public async Task<GetEmployeeResponse> UpdateEmployee(Guid id, UpdateEmployeeRequest dto)
        {
            var employee = await _unitOfWork.Employees.Get(id);

            // validate
            if (employee is null) throw new KeyNotFoundException();

            if (!string.IsNullOrEmpty(dto.FirstName) && employee.FirstName != dto.FirstName)
                employee.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName) && employee.LastName != dto.LastName)
                employee.LastName = dto.LastName;

            _unitOfWork.Commit();
            return _mapper.Map<GetEmployeeResponse>(employee);
        }

        public async Task<GetEmployeeWithSpecializationsResponse> AddSpecialization(Guid employeeId, Guid specializationId)
        {
            var employee = await _unitOfWork.Employees.GetWithSpecializations(employeeId);
            var specialization = await _unitOfWork.Specializations.Get(specializationId);

            // validate
            if (employee is null)
                throw new KeyNotFoundException("Employee does not exist.");

            if (specialization is null)
                throw new KeyNotFoundException("Specialization does not exist.");

            if (employee.EmployeeSpecializations.Any(es => es.SpecializationId == specialization.Id))
                throw new AppException($"Specialization {specialization.Name} is already assigned to this employee.");

            employee.EmployeeSpecializations.Add(new EmployeeSpecialization
            {
                EmployeeId = employee.Id,
                SpecializationId = specialization.Id
            });

            _unitOfWork.Commit();

            return await GetEmployeeById(employee.Id);
        }

        public async void RemoveSpecialization(Guid employeeId, Guid specializationId)
        {
            var employee = await _unitOfWork.Employees.GetWithSpecializations(employeeId);

            // validate
            if (employee is null)
                throw new KeyNotFoundException("Employee does not exist.");

            var isRemoved = employee.EmployeeSpecializations.Remove(new EmployeeSpecialization
            {
                EmployeeId = employeeId,
                SpecializationId = specializationId
            });

            if (!isRemoved)
                throw new KeyNotFoundException("Specialization not found in employee specializations.");

            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
