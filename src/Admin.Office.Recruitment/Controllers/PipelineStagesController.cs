using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Services;
using Admin.Office.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Office.Recruitment.Controllers;

[ApiController]
[Route("api/recruitment/[controller]")]
public class PipelineStagesController(IPipelineStageService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PipelineStageDto>>>> GetStages()
    {
        var stages = await service.GetStagesAsync();
        return Ok(ApiResponse<List<PipelineStageDto>>.Ok(stages));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PipelineStageDto>>> GetStage(Guid id)
    {
        var stage = await service.GetStageByIdAsync(id);
        if (stage == null)
            return NotFound(ApiResponse<PipelineStageDto>.Fail("Stage not found"));
        return Ok(ApiResponse<PipelineStageDto>.Ok(stage));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PipelineStageDto>>> CreateStage(CreatePipelineStageDto dto)
    {
        var stage = await service.CreateStageAsync(dto);
        return CreatedAtAction(nameof(GetStage), new { id = stage.Id },
            ApiResponse<PipelineStageDto>.Ok(stage, "Stage created"));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<PipelineStageDto>>> UpdateStage(Guid id, UpdatePipelineStageDto dto)
    {
        var stage = await service.UpdateStageAsync(id, dto);
        if (stage == null)
            return NotFound(ApiResponse<PipelineStageDto>.Fail("Stage not found"));
        return Ok(ApiResponse<PipelineStageDto>.Ok(stage, "Stage updated"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteStage(Guid id)
    {
        var deleted = await service.DeleteStageAsync(id);
        if (!deleted)
            return NotFound(ApiResponse<bool>.Fail("Stage not found"));
        return Ok(ApiResponse<bool>.Ok(true, "Stage deleted"));
    }
}
