using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.Recruitment.Services;

public class JobPositionService(DbContext context) : IJobPositionService
{
    private DbSet<JobPosition> Positions => context.Set<JobPosition>();
    private DbSet<RecruitmentDepartment> Departments => context.Set<RecruitmentDepartment>();

    public async Task<List<RecruitmentDepartmentDto>> GetDepartmentsWithPositionsAsync(Guid? departmentId = null)
    {
        var query = Departments
            .Include(d => d.JobPositions).ThenInclude(jp => jp.Applicants)
            .AsQueryable();

        if (departmentId.HasValue)
            query = query.Where(d => d.Id == departmentId.Value);

        var departments = await query.ToListAsync();
        return departments.Select(MapDepartmentDto).ToList();
    }

    public async Task<JobPositionDto?> GetJobPositionByIdAsync(Guid id)
    {
        var jp = await Positions
            .Include(j => j.Department)
            .Include(j => j.Applicants)
            .FirstOrDefaultAsync(j => j.Id == id);

        return jp == null ? null : MapPositionDto(jp);
    }

    public async Task<JobPositionDto> CreateJobPositionAsync(CreateJobPositionDto dto)
    {
        var jp = new JobPosition
        {
            Title = dto.Title,
            DepartmentId = dto.DepartmentId,
            ResponsiblePerson = dto.ResponsiblePerson,
            ToRecruit = dto.ToRecruit
        };

        Positions.Add(jp);
        await context.SaveChangesAsync();

        await context.Entry(jp).Reference(j => j.Department).LoadAsync();
        return MapPositionDto(jp);
    }

    public async Task<JobPositionDto?> UpdateJobPositionAsync(Guid id, UpdateJobPositionDto dto)
    {
        var jp = await Positions
            .Include(j => j.Department)
            .Include(j => j.Applicants)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jp == null) return null;

        if (dto.Title != null) jp.Title = dto.Title;
        if (dto.DepartmentId.HasValue) jp.DepartmentId = dto.DepartmentId.Value;
        if (dto.ResponsiblePerson != null) jp.ResponsiblePerson = dto.ResponsiblePerson;
        if (dto.ToRecruit.HasValue) jp.ToRecruit = dto.ToRecruit.Value;

        jp.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapPositionDto(jp);
    }

    public async Task<bool> DeleteJobPositionAsync(Guid id)
    {
        var jp = await Positions.FindAsync(id);
        if (jp == null) return false;

        Positions.Remove(jp);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<RecruitmentDepartmentDto>> GetRecruitmentDepartmentsAsync()
    {
        var depts = await Departments
            .Include(d => d.JobPositions).ThenInclude(jp => jp.Applicants)
            .ToListAsync();
        return depts.Select(MapDepartmentDto).ToList();
    }

    public async Task<RecruitmentDepartmentDto> CreateRecruitmentDepartmentAsync(CreateRecruitmentDepartmentDto dto)
    {
        var dept = new RecruitmentDepartment { Name = dto.Name };
        Departments.Add(dept);
        await context.SaveChangesAsync();
        return MapDepartmentDto(dept);
    }

    public async Task<RecruitmentDepartmentDto?> UpdateRecruitmentDepartmentAsync(Guid id, UpdateRecruitmentDepartmentDto dto)
    {
        var dept = await Departments.Include(d => d.JobPositions).FirstOrDefaultAsync(d => d.Id == id);
        if (dept == null) return null;

        if (dto.Name != null) dept.Name = dto.Name;
        dept.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapDepartmentDto(dept);
    }

    public async Task<bool> DeleteRecruitmentDepartmentAsync(Guid id)
    {
        var dept = await Departments.FindAsync(id);
        if (dept == null) return false;

        Departments.Remove(dept);
        await context.SaveChangesAsync();
        return true;
    }

    private static JobPositionDto MapPositionDto(JobPosition jp) => new(
        jp.Id, jp.Title, jp.DepartmentId,
        jp.Department?.Name ?? "",
        jp.ResponsiblePerson,
        jp.Applicants.Count(a => a.AppliedDate >= DateTime.UtcNow.AddDays(-30)),
        jp.ToRecruit,
        jp.Applicants.Count
    );

    private static RecruitmentDepartmentDto MapDepartmentDto(RecruitmentDepartment d) => new(
        d.Id, d.Name, d.JobPositions.Count,
        d.JobPositions.Select(MapPositionDto).ToList()
    );
}
