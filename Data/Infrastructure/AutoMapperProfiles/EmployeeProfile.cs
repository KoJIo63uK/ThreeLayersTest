using AutoMapper;
using Domain.Entity;

namespace Data.Infrastructure.AutoMapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDomain, Employee>().ReverseMap();
        }
    }
}