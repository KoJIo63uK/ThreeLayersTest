using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces.ViewLayer;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Present.Controllers;
using Present.Entity.Employee;
using Xunit;

namespace PresentTests
{
    public class EmployeeControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEmployeeService> _mock;
        private readonly IEmployeeService _service;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            var mapperConfig = new MapperConfiguration(conf =>
                conf.AddProfile(typeof(Present.Infrastructure.AutomapperProfiles.EmployeeProfile)));
            _mapper = new Mapper(mapperConfig);
            
            _mock = new Mock<IEmployeeService>();
            ConfigMoq();
            
            _service = new EmployeeService(_mock.Object);
            
            _controller = new EmployeeController(_mapper, _service);
        }

        [Fact]
        public async Task GetAllTestAsync()
        {
            var result = await _controller.GetAllAsync();
            var expected = result as ObjectResult;
            var actual = GetTestEmployeesView();
            
            Assert.Equal(StatusCodes.Status200OK, expected.StatusCode);
            Assert.Equal(expected.Value, actual);
        }
        
        [Fact]
        public async Task GetByIdTest()
        {
            var expexted = GetTestEmployeeDomain();
            var actual = await _controller.GetEmploeeAsync("b33b6b59-d015-48ba-8f1f-c4acb8936023");
            var result = actual as ObjectResult;
            
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(expexted, result.Value);
        }
        
        [Fact]
        public async Task GetByIdBadRequestTest()
        {
            var actual = await _controller.GetEmploeeAsync("33bb59-d015-48ba-8f1f-c4acb8936023");
            var result = actual as ObjectResult;
            
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
        
        [Fact]
        public async Task GetByIdNotFoundTest()
        {
            var actual = await _controller.GetEmploeeAsync("b33b6b59-d015-48ba-8f1f-c4acb8936028");
            var result = actual as ObjectResult;

            if (result != null) Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }

        [Fact]
        public async Task AddEmployeeTest()
        {
            var expected = GetNewEmployeeDomain();
            var actual = await _controller.AddAsync(GetRawEmployeeView());
            var result = actual as ObjectResult;
            
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal(expected, result.Value);
        }

        [Fact]
        public async Task UpdateEmployeeDomain()
        {
            var expected = GetUpdateEmployeeView();
            var actual = await _controller.UpdateAsync("b33b6b59-d015-48ba-8f1f-c4acb8936023", GetRawEmployeeView());
            var result = actual as ObjectResult;
            
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
            Assert.Equal(expected, result.Value);
        }
        
        [Fact]
        public async Task UpdateBadRequestTest()
        {
            var actual = await _controller.UpdateAsync("b33b659-d015-48ba-8f1f-c4acb8936023", GetRawEmployeeView());
            var result = actual as ObjectResult;
            
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }
        
        [Fact]
        public async Task UpdateNotFoundTest()
        {
            var actual = await _controller.UpdateAsync("b33b6b59-d015-48ba-8f1f-c4acb8936029", GetRawEmployeeView());
            var result = actual as ObjectResult;

            if (result != null) Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        }
        
        [Fact]
        public async Task DeleteTest()
        {
            _mock.Setup(e =>
                e.DeleteAsync(It.IsAny<Guid>()));

            var result = await _controller.DeleteAsync("b33b6b59-d015-48ba-8f1f-c4acb8936024");
            var actual = result as ObjectResult;
            
            _mock.Verify(e => 
                e.DeleteAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024")));

            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
        }
        
        [Fact]
        public async Task DeleteBadRequestTest()
        {
            _mock.Setup(e =>
                e.DeleteAsync(It.IsAny<Guid>()));

            var result = await _controller.DeleteAsync("b33b6b59-d015-48ba-8ff-c4acb8936024");
            var actual = result as ObjectResult;
            
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        private void ConfigMoq()
        {
            _mock.Setup(e => e.GetAllAsync())
                .Returns(Task.FromResult<IList<EmployeeDomain>>(GetTestEmployeesDomain()));
            _mock.Setup(e => e.GetByIdAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023")))
                .Returns(Task.FromResult<EmployeeDomain>(GetTestEmployeeDomain()));
            _mock.Setup(e => e.AddAsync(GetRawEmployeeDomain()))
                .Returns(Task.FromResult<EmployeeDomain>(GetNewEmployeeDomain()));
            _mock.Setup(e => e.UpdateAsync(GetUpdateEmployeeDomain()))
                .Returns(Task.FromResult<EmployeeDomain>(GetUpdateEmployeeDomain()));
        }
        
        #region DomainEmployee

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
        
        private EmployeeDomain GetTestEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                Name = "Vlad",
                Surname = "Vorosalov"
            };
        }
        
        private EmployeeDomain GetNewEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936025"),
                Name = "Olen",
                Surname = "Ivanov"
            };
        }
        
        private EmployeeDomain GetUpdateEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                Name = "Olen",
                Surname = "Ivanov"
            };
        }

        #endregion
        
        #region ViewEmployee

        private List<EmployeeView> GetTestEmployeesView()
        {
            return new List<EmployeeView>
            {
                new EmployeeView()
                {
                    Id = "b33b6b59-d015-48ba-8f1f-c4acb8936023",
                    Name = "Vlad",
                    Surname = "Vorosalov"
                },
                new EmployeeView()
                {
                    Id = "b33b6b59-d015-48ba-8f1f-c4acb8936024",
                    Name = "Olya",
                    Surname = "Vorosalova"
                }
            };
        }
        
        private EmployeeView GetTestEmployeeView()
        {
            return new EmployeeView()
            {
                Id = "b33b6b59-d015-48ba-8f1f-c4acb8936023",
                Name = "Vlad",
                Surname = "Vorosalov"
            };
        }
        
        private EmployeeView GetNewEmployeeView()
        {
            return new EmployeeView()
            {
                Id = "b33b6b59-d015-48ba-8f1f-c4acb8936025",
                Name = "Olen",
                Surname = "Ivanov"
            };
        }
        
        private EmployeeView GetUpdateEmployeeView()
        {
            return new EmployeeView()
            {
                Id = "b33b6b59-d015-48ba-8f1f-c4acb8936023",
                Name = "Olen",
                Surname = "Ivanov"
            };
        }

        #endregion

        #region RawEmployees

        private EmployeeDomain GetRawEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Name = "Olen",
                Surname = "Ivanov"
            };
        }
        
        private EmployeeRaw GetRawEmployeeView()
        {
            return new EmployeeRaw()
            {
                Name = "Olen",
                Surname = "Ivanov"
            };
        }

        #endregion
    }
}