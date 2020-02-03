using AutoMapper;
using Domain.Entity;
using Present.Entity.Employee;

namespace Present.Infrastructure.AutomapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDomain, EmployeeView>();
            CreateMap<EmployeeRaw, EmployeeDomain>();
        }   
    }
}