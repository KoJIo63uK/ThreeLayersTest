using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces.DataAccess;
using Microsoft.EntityFrameworkCore;
using AppContext = Data.Context.AppContext;

namespace Data.Services
{
    public class EmployeeDbService : IEmployeeDbService
    {
        private readonly IMapper _mapper;
        private readonly AppContext _context;

        public EmployeeDbService(IMapper mapper, AppContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<IList<EmployeeDomain>> GetAllAsync()
        {
            var employees = await _context.Employees.ToListAsync();
            return _mapper.Map<List<EmployeeDomain>>(employees);
        }
        
        public async Task<EmployeeDomain> AddAsync(EmployeeDomain employeeDomain)
        {
            var employee = await _context.AddAsync(_mapper.Map<Employee>(employeeDomain));
            await _context.SaveChangesAsync();
            return _mapper.Map<EmployeeDomain>(employee.Entity);
        }
        
        public async Task<EmployeeDomain> GetByIdAsync(Guid id)
        {
            var employee = await GetDbEmployee(id);
            
            await _context.SaveChangesAsync();
            
            return _mapper.Map<EmployeeDomain>(employee);
        }
        
        public async Task<EmployeeDomain> UpdateAsync(EmployeeDomain employeeDomain)
        {
            var employee = _mapper.Map<Employee>(employeeDomain);

            var result = _context.Update(employee).Entity;
            
            await _context.SaveChangesAsync();
            
            return _mapper.Map<EmployeeDomain>(result);
        }
        
        public async Task DeleteAsync(Guid guid)
        {
            var employee = await GetDbEmployee(guid);
            _context.Remove(employee);
            await _context.SaveChangesAsync();
        }
        
        private async Task<Employee> GetDbEmployee(Guid id)
        {
            return await _context.Employees.SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}