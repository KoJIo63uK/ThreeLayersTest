using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Interfaces.ViewLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Present.Entity.Employee;

namespace Present.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _service;

        public EmployeeController(IMapper mapper, IEmployeeService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EmployeeView>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var employees = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<EmployeeView>>(employees));
        }

        [HttpGet("/employees/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmploeeAsync(string id)
        {
            try
            {
                var guid = Guid.Parse(id);
                var employee = await _service.GetByIdAsync(guid);

                if (employee == null) return NotFound("Employee not found");
                
                return Ok(employee);
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpPost("/employees")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EmployeeView))]
        public async Task<IActionResult> AddAsync([FromBody] EmployeeRaw employeeRaw)
        {
            var employee = _mapper.Map<EmployeeDomain>(employeeRaw);
            var result = await _service.AddAsync(employee);
            
            return Created($"/employees/{result.Id}",result);
        }

        [HttpPut("/employees/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeView))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] EmployeeRaw employeeRaw)
        {
            try
            {
                var guid = Guid.Parse(id);
                var employee = _mapper.Map<EmployeeDomain>(employeeRaw);
                employee.Id = guid;

                var result = await _service.UpdateAsync(employee);

                if (result == null) return NotFound("Employee no exist");

                return Ok(_mapper.Map<EmployeeView>(result));
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("/employees/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var guid = Guid.Parse(id);
                await _service.DeleteAsync(guid);

                return Ok("Employee was deleted");
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
            }
        }
        
    }
}