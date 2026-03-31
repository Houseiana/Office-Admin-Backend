using Admin.Office.HumanResources.DTOs;
using Admin.Office.HumanResources.Services;
using Admin.Office.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Office.HumanResources.Controllers;

[ApiController]
[Route("api/hr/[controller]")]
public class EmployeesController(IEmployeeService employeeService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<EmployeeDto>>>> GetEmployees(
        [FromQuery] string? search,
        [FromQuery] string? department,
        [FromQuery] string? groupBy,
        [FromQuery] List<string>? filters,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await employeeService.GetEmployeesAsync(search, department, groupBy, filters, page, pageSize);
        return Ok(ApiResponse<PagedResponse<EmployeeDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetEmployee(Guid id)
    {
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
            return NotFound(ApiResponse<EmployeeDto>.Fail("Employee not found"));

        return Ok(ApiResponse<EmployeeDto>.Ok(employee));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> CreateEmployee(CreateEmployeeDto dto)
    {
        var employee = await employeeService.CreateEmployeeAsync(dto);
        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id },
            ApiResponse<EmployeeDto>.Ok(employee, "Employee created"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> UpdateEmployee(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await employeeService.UpdateEmployeeAsync(id, dto);
        if (employee == null)
            return NotFound(ApiResponse<EmployeeDto>.Fail("Employee not found"));

        return Ok(ApiResponse<EmployeeDto>.Ok(employee, "Employee updated"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteEmployee(Guid id)
    {
        var deleted = await employeeService.DeleteEmployeeAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Employee not found"));

        return Ok(ApiResponse<bool>.Ok(true, "Employee deleted"));
    }

    [HttpGet("reporting")]
    public async Task<ActionResult<ApiResponse<ReportingStatsDto>>> GetReportingStats()
    {
        var stats = await employeeService.GetReportingStatsAsync();
        return Ok(ApiResponse<ReportingStatsDto>.Ok(stats));
    }
}
