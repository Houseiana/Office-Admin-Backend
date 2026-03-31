using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Services;
using Admin.Office.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Office.Recruitment.Controllers;

[ApiController]
[Route("api/recruitment/[controller]")]
public class ApplicantsController(IApplicantService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<ApplicantListDto>>>> GetApplicants(
        [FromQuery] Guid? jobPositionId,
        [FromQuery] Guid? stageId,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await service.GetApplicantsAsync(jobPositionId, stageId, search, page, pageSize);
        return Ok(ApiResponse<PagedResponse<ApplicantListDto>>.Ok(result));
    }

    [HttpGet("by-stage")]
    public async Task<ActionResult<ApiResponse<List<ApplicantListDto>>>> GetApplicantsByStage(
        [FromQuery] Guid? jobPositionId)
    {
        var result = await service.GetApplicantsByStageAsync(jobPositionId);
        return Ok(ApiResponse<List<ApplicantListDto>>.Ok(result));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ApplicantDetailDto>>> GetApplicant(Guid id)
    {
        var applicant = await service.GetApplicantByIdAsync(id);
        if (applicant == null)
            return NotFound(ApiResponse<ApplicantDetailDto>.Fail("Applicant not found"));
        return Ok(ApiResponse<ApplicantDetailDto>.Ok(applicant));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ApplicantDetailDto>>> CreateApplicant(CreateApplicantDto dto)
    {
        var applicant = await service.CreateApplicantAsync(dto);
        return CreatedAtAction(nameof(GetApplicant), new { id = applicant.Id },
            ApiResponse<ApplicantDetailDto>.Ok(applicant, "Applicant created"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ApplicantDetailDto>>> UpdateApplicant(Guid id, UpdateApplicantDto dto)
    {
        var applicant = await service.UpdateApplicantAsync(id, dto);
        if (applicant == null)
            return NotFound(ApiResponse<ApplicantDetailDto>.Fail("Applicant not found"));
        return Ok(ApiResponse<ApplicantDetailDto>.Ok(applicant, "Applicant updated"));
    }

    [HttpPatch("{id:guid}/move")]
    public async Task<ActionResult<ApiResponse<ApplicantDetailDto>>> MoveApplicant(Guid id, MoveApplicantDto dto)
    {
        var applicant = await service.MoveApplicantToStageAsync(id, dto);
        if (applicant == null)
            return NotFound(ApiResponse<ApplicantDetailDto>.Fail("Applicant not found"));
        return Ok(ApiResponse<ApplicantDetailDto>.Ok(applicant, "Applicant moved"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteApplicant(Guid id)
    {
        var deleted = await service.DeleteApplicantAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Applicant not found"));
        return Ok(ApiResponse<bool>.Ok(true, "Applicant deleted"));
    }
}
