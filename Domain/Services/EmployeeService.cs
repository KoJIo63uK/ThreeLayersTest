using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces.DataAccess;
using Domain.Interfaces.ViewLayer;

namespace Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeDbService _service;

        public EmployeeService(IEmployeeDbService service)
        {
            _service = service;
        }

        public async Task<IList<EmployeeDomain>> GetAllAsync()
        {
            return await _service.GetAllAsync();
        }

        public async Task<EmployeeDomain> GetByIdAsync(Guid id)
        {
            return await _service.GetByIdAsync(id);
        }

        public async Task<EmployeeDomain> AddAsync(EmployeeDomain employeeDomain)
        {
            return await _service.AddAsync(employeeDomain);
        }

        public async Task<EmployeeDomain> UpdateAsync(EmployeeDomain employeeDomain)
        {
            return await _service.UpdateAsync(employeeDomain);
        }

        public async Task DeleteAsync(Guid guid)
        {
            await _service.DeleteAsync(guid);
        }
    }
}