using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Services;
using Admin.Office.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Office.Recruitment.Controllers;

[ApiController]
[Route("api/recruitment/[controller]")]
public class ReportingController(IReportingService service) : ControllerBase
{
    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<RecruitmentStatsDto>>> GetStats()
    {
        var stats = await service.GetStatsAsync();
        return Ok(ApiResponse<RecruitmentStatsDto>.Ok(stats));
    }

    [HttpGet("trends")]
    public async Task<ActionResult<ApiResponse<List<RecruitmentTrendDto>>>> GetTrends([FromQuery] int months = 6)
    {
        var trends = await service.GetTrendsAsync(months);
        return Ok(ApiResponse<List<RecruitmentTrendDto>>.Ok(trends));
    }
}
