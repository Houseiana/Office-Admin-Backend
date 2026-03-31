using Admin.Office.Recruitment.DTOs;

namespace Admin.Office.Recruitment.Services;

public interface IReportingService
{
    Task<RecruitmentStatsDto> GetStatsAsync();
    Task<List<RecruitmentTrendDto>> GetTrendsAsync(int months = 6);
}
