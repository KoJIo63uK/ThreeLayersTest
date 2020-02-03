using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interfaces.DataAccess;
using Domain.Interfaces.ViewLayer;
using Domain.Services;
using Moq;
using Xunit;

namespace DmainTests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeDbService> _mock;
        private readonly IEmployeeService _service;

        public EmployeeServiceTests()
        {
            _mock = new Mock<IEmployeeDbService>();
            _mock.Setup(e => 
                e.GetAllAsync()).Returns(Task.FromResult<IList<EmployeeDomain>>(GetTestEmployeesDomain()));
            
            _mock.Setup(e =>
                    e.GetByIdAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024")))
                .Returns(Task.FromResult<EmployeeDomain>(GetTestEmployeeDomain()));
            
            _mock.Setup(e => e.AddAsync(GetRawEmployeeDomain()))
                .Returns(Task.FromResult<EmployeeDomain>(GetNewEmployeeDomain()));

            _mock.Setup(e => e.UpdateAsync(GetUpdateEmployeeDomain()))
                .Returns(Task.FromResult<EmployeeDomain>(GetUpdateEmployeeDomain()));
            
            _service = new EmployeeService(_mock.Object);
        }
        
        [Fact]
        public async Task GetAllTest()
        {
            var expected = GetTestEmployeesDomain();
            var actual = await _service.GetAllAsync();
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByIdTest()
        {
            var expexted = GetTestEmployeeDomain();
            var actual = await _service.GetByIdAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024"));
            
            Assert.Equal(expexted, actual);
        }

        [Fact]
        public async Task AddEmployeeTest()
        {
            var expected = GetNewEmployeeDomain();
            var actual = await _service.AddAsync(GetRawEmployeeDomain());
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task UpdateEmployeeDomain()
        {
            var expected = GetUpdateEmployeeDomain();
            var actual = await _service.UpdateAsync(GetUpdateEmployeeDomain());
            
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task DeleteTest()
        {
            _mock.Setup(e =>
                e.DeleteAsync(It.IsAny<Guid>()));

            await _service.DeleteAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024"));
            
            _mock.Verify(e => 
                e.DeleteAsync(Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936024")));
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
        
        private EmployeeDomain GetTestEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Id = Guid.Parse("b33b6b59-d015-48ba-8f1f-c4acb8936023"),
                Name = "Vlad",
                Surname = "Vorosalov"
            };
        }

        private EmployeeDomain GetRawEmployeeDomain()
        {
            return new EmployeeDomain()
            {
                Name = "Olen",
                Surname = "Ivanov"
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
    }
}