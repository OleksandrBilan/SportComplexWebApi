﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.Employee;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginApiModel)
        {
            var employee = await _employeeService.LoginAsync(loginApiModel.Login, loginApiModel.Password);

            if (employee is null)
            {
                return BadRequest();
            }

            return Ok(employee);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _employeeService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeDto employee)
        {
            var createdEmployee = await _employeeService.CreateAsync(employee);

            if (createdEmployee is null)
            {
                return BadRequest();
            }

            return Ok(createdEmployee);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] EmployeeDto employee)
        {
            var updatedEmployee = await _employeeService.UpdateAsync(employee);

            if (updatedEmployee is null)
            {
                return BadRequest();
            }

            return Ok(updatedEmployee);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _employeeService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("getPositionTypes")]
        public async Task<IActionResult> GetPositionTypesAsync()
        {
            return Ok(await _employeeService.GetPositionTypesAsync());
        }

        #region Employee Education

        [HttpGet("getEducationLevels")]
        public async Task<IActionResult> GetEducationLevelsAsync()
        {
            return Ok(await _employeeService.GetEducationLevelsAsync());
        }

        [HttpPost("addEmployeeEducation")]
        public async Task<IActionResult> AddEmployeeEducationAsync([FromBody] EducationDto education)
        {
            await _employeeService.UpsertEmployeeEducationAsync(education);
            return Ok();
        }

        [HttpPut("updateEmployeeEducation")]
        public async Task<IActionResult> UpdateEmployeeEducationAsync([FromBody] EducationDto education)
        {
            await _employeeService.UpsertEmployeeEducationAsync(education);
            return Ok();
        }

        [HttpDelete("deleteEmployeeEducation")]
        public async Task<IActionResult> DeleteEmployeeEducationAsync(int id)
        {
            await _employeeService.DeleteEmployeeEducationAsync(id);
            return Ok();
        }

        #endregion

        #region Previus Jobs

        [HttpPost("addEmployeePreviousJob")]
        public async Task<IActionResult> AddEmployeePreviousJobAsync([FromBody] PreviousJobDto job)
        {
            await _employeeService.UpsertEmployeePreviousJobAsync(job);
            return Ok();
        }

        [HttpPut("updateEmployeePreviousJob")]
        public async Task<IActionResult> UpdateEmployeePreviousJobAsync([FromBody] PreviousJobDto job)
        {
            await _employeeService.UpsertEmployeePreviousJobAsync(job);
            return Ok();
        }

        [HttpDelete("deleteEmployeePreviousJob")]
        public async Task<IActionResult> DeleteEmployeePreviousJobAsync(int id)
        {
            await _employeeService.DeleteEmployeeEducationAsync(id);
            return Ok();
        }

        #endregion
    }
}
