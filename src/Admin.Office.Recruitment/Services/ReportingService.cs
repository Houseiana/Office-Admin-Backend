using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.Recruitment.Services;

public class ReportingService(DbContext context) : IReportingService
{
    private DbSet<Applicant> Applicants => context.Set<Applicant>();
    private DbSet<JobPosition> Positions => context.Set<JobPosition>();

    public async Task<RecruitmentStatsDto> GetStatsAsync()
    {
        var totalApplicants = await Applicants.CountAsync();
        var openPositions = await Positions.CountAsync(p => p.ToRecruit > 0);
        var newApplications = await Applicants.CountAsync(a => a.AppliedDate >= DateTime.UtcNow.AddDays(-30));
        var toRecruit = await Positions.SumAsync(p => p.ToRecruit);

        return new RecruitmentStatsDto(totalApplicants, openPositions, newApplications, toRecruit);
    }

    public async Task<List<RecruitmentTrendDto>> GetTrendsAsync(int months = 6)
    {
        var startDate = DateTime.UtcNow.AddMonths(-months);

        var applicants = await Applicants
            .Include(a => a.JobPosition)
            .Where(a => a.AppliedDate >= startDate)
            .ToListAsync();

        var trends = applicants
            .GroupBy(a => a.AppliedDate.ToString("yyyy-MM"))
            .OrderBy(g => g.Key)
            .Select(g => new RecruitmentTrendDto(
                g.Key,
                g.GroupBy(a => a.JobPosition?.Title ?? "Unknown")
                    .ToDictionary(pg => pg.Key, pg => pg.Count())
            ))
            .ToList();

        return trends;
    }
}
