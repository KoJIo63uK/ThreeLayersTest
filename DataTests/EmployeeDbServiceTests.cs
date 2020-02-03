using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Data.Services;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using AppContext = Data.Context.AppContext;

namespace DataTests
{
    public class EmployeeDbServiceTests
    {
        private readonly IMapper _mapper;
        private readonly AppContext _appContext;
        private List<EmployeeDomain>expectEmployeeDomain;
        
        public EmployeeDbServiceTests()
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<Employee,EmployeeDomain>().ReverseMap());
            
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            
            
            
            var options = new DbContextOptionsBuilder<AppContext>();
            options.UseInMemoryDatabase("TestDatabase");
            options.UseInternalServiceProvider(serviceProvider);
            
            _mapper = new Mapper(config);
            _appContext = new AppContext(options.Options);
        }

        [Fact]
        public async Task GetAll()
        {
            //Arrage
            var service = new EmployeeDbService(_mapper, _appContext);

            //Act
            var result = await service.GetAllAsync();
            
            //Assert
            Assert.Equal(GetTestEmployeesDomain(), result);
            
        }

        [Fact]
        public async Task GetById()
        {
            //Arrage
            var service = new EmployeeDbService(_mapper, _appContext);

            //Act
            var result = await service.GetByIdAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"));
            
            //Assert
            Assert.Equal(GetTestEmployeeDomain(), result);
        }

        [Fact]
        public async Task Add()
        {
            //Arrage
            var service = new EmployeeDbService(_mapper, _appContext);
            var newEmployee = new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936025"),
                Name = "Olen",
                Surname = "Ivanov"
            };

            //Act
            var result = await service.AddAsync(newEmployee);

            //Assert
            Assert.Equal(3, _appContext.Employees.Count());
            Assert.Equal(newEmployee, result);
            result = await service.GetByIdAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936025"));
            Assert.Equal(newEmployee, result);
            
        }

        [Fact]
        public async Task Update()
        {
            //Arrage
            var service = new EmployeeDbService(_mapper, _appContext);
            var newEmployee = new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                Name = "Olen",
                Surname = "Ivanov"
            };

            //Act
            var result = await service.UpdateAsync(newEmployee);

            //Assert
            Assert.Equal(2, _appContext.Employees.Count());
            Assert.Equal(newEmployee, result);
            result = await service.GetByIdAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"));
            Assert.Equal(newEmployee, result);
        }

        [Fact]
        public async Task Delete()
        {
            var service = new EmployeeDbService(_mapper, _appContext);

            await service.DeleteAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"));
            
            Assert.Equal(1, _appContext.Employees.Count());
        }

        private List<Employee> GetTestEmployees()
        {
            return new List<Employee>
            {
                new Employee()
                {
                    Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                    Name = "Vlad",
                    Surname = "Vorosalov"
                },
                new Employee()
                {
                    Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024"),
                    Name = "Olya",
                    Surname = "Vorosalova"
                }
            };
        }

        private List<EmployeeDomain> GetTestEmployeesDomain()
        {
            return new List<EmployeeDomain>
            {
                new EmployeeDomain()
                {
                    Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                    Name = "Vlad",
                    Surname = "Vorosalov"
                },
                new EmployeeDomain()
                {
                    Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024"),
                    Name = "Olya",
                    Surname = "Vorosalova"
                }
            };
        }

        private Employee GetTestEmployee()
        {
            return new Employee()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                Name = "Vlad",
                Surname = "Vorosalov"
            };
        }
        
        private EmployeeDomain GetTestEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                Name = "Vlad",
                Surname = "Vorosalov"
            };
        }
    }
}