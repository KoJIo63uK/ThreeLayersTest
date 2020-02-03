using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Interfaces.DataAccess
{
    public interface IEmployeeDbService
    {
        Task<IList<EmployeeDomain>> GetAllAsync();
        Task<EmployeeDomain> GetByIdAsync(Guid id);
        Task<EmployeeDomain> AddAsync(EmployeeDomain employeeDomain);
        Task<EmployeeDomain> UpdateAsync(EmployeeDomain employeeDomain);
        Task DeleteAsync(Guid guid);
    }
}