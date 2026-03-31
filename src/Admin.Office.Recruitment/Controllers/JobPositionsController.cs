using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Services;
using Admin.Office.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Office.Recruitment.Controllers;

[ApiController]
[Route("api/recruitment/[controller]")]
public class JobPositionsController(IJobPositionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<RecruitmentDepartmentDto>>>> GetDepartmentsWithPositions(
        [FromQuery] Guid? departmentId)
    {
        var result = await service.GetDepartmentsWithPositionsAsync(departmentId);
        return Ok(ApiResponse<List<RecruitmentDepartmentDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<JobPositionDto>>> GetJobPosition(Guid id)
    {
        var jp = await service.GetJobPositionByIdAsync(id);
        if (jp == null)
            return NotFound(ApiResponse<JobPositionDto>.Fail("Job position not found"));
        return Ok(ApiResponse<JobPositionDto>.Ok(jp));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<JobPositionDto>>> CreateJobPosition(CreateJobPositionDto dto)
    {
        var jp = await service.CreateJobPositionAsync(dto);
        return CreatedAtAction(nameof(GetJobPosition), new { id = jp.Id },
            ApiResponse<JobPositionDto>.Ok(jp, "Job position created"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<JobPositionDto>>> UpdateJobPosition(Guid id, UpdateJobPositionDto dto)
    {
        var jp = await service.UpdateJobPositionAsync(id, dto);
        if (jp == null)
            return NotFound(ApiResponse<JobPositionDto>.Fail("Job position not found"));
        return Ok(ApiResponse<JobPositionDto>.Ok(jp, "Job position updated"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteJobPosition(Guid id)
    {
        var deleted = await service.DeleteJobPositionAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Job position not found"));
        return Ok(ApiResponse<bool>.Ok(true, "Job position deleted"));
    }

    [HttpGet("departments")]
    public async Task<ActionResult<ApiResponse<List<RecruitmentDepartmentDto>>>> GetDepartments()
    {
        var depts = await service.GetRecruitmentDepartmentsAsync();
        return Ok(ApiResponse<List<RecruitmentDepartmentDto>>.Ok(depts));
    }

    [HttpPost("departments")]
    public async Task<ActionResult<ApiResponse<RecruitmentDepartmentDto>>> CreateDepartment(CreateRecruitmentDepartmentDto dto)
    {
        var dept = await service.CreateRecruitmentDepartmentAsync(dto);
        return Ok(ApiResponse<RecruitmentDepartmentDto>.Ok(dept, "Department created"));
    }

    [HttpPut("departments/{id:guid}")]
    public async Task<ActionResult<ApiResponse<RecruitmentDepartmentDto>>> UpdateDepartment(Guid id, UpdateRecruitmentDepartmentDto dto)
    {
        var dept = await service.UpdateRecruitmentDepartmentAsync(id, dto);
        if (dept == null)
            return NotFound(ApiResponse<RecruitmentDepartmentDto>.Fail("Department not found"));
        return Ok(ApiResponse<RecruitmentDepartmentDto>.Ok(dept, "Department updated"));
    }

    [HttpDelete("departments/{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteDepartment(Guid id)
    {
        var deleted = await service.DeleteRecruitmentDepartmentAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Department not found"));
        return Ok(ApiResponse<bool>.Ok(true, "Department deleted"));
    }
}
