using Admin.Office.HumanResources.DTOs;
using Admin.Office.HumanResources.Services;
using Admin.Office.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Office.HumanResources.Controllers;

[ApiController]
[Route("api/hr/[controller]")]
public class DepartmentsController(IDepartmentService departmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<DepartmentDto>>>> GetDepartments()
    {
        var departments = await departmentService.GetDepartmentsAsync();
        return Ok(ApiResponse<List<DepartmentDto>>.Ok(departments));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetDepartment(Guid id)
    {
        var dept = await departmentService.GetDepartmentByIdAsync(id);
        if (dept == null)
            return NotFound(ApiResponse<DepartmentDto>.Fail("Department not found"));

        return Ok(ApiResponse<DepartmentDto>.Ok(dept));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> CreateDepartment(CreateDepartmentDto dto)
    {
        var dept = await departmentService.CreateDepartmentAsync(dto);
        return CreatedAtAction(nameof(GetDepartment), new { id = dept.Id },
            ApiResponse<DepartmentDto>.Ok(dept, "Department created"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> UpdateDepartment(Guid id, UpdateDepartmentDto dto)
    {
        var dept = await departmentService.UpdateDepartmentAsync(id, dto);
        if (dept == null)
            return NotFound(ApiResponse<DepartmentDto>.Fail("Department not found"));

        return Ok(ApiResponse<DepartmentDto>.Ok(dept, "Department updated"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDepartment(Guid id)
    {
        var deleted = await departmentService.DeleteDepartmentAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Department not found"));

        return Ok(ApiResponse<bool>.Ok(true, "Department deleted"));
    }
}
