using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Models;
using Admin.Office.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.Recruitment.Services;

public class ApplicantService(DbContext context) : IApplicantService
{
    private DbSet<Applicant> Applicants => context.Set<Applicant>();

    public async Task<PagedResponse<ApplicantListDto>> GetApplicantsAsync(
        Guid? jobPositionId, Guid? stageId, string? search, int page = 1, int pageSize = 20)
    {
        var query = Applicants
            .Include(a => a.JobPosition)
            .Include(a => a.Stage)
            .AsQueryable();

        if (jobPositionId.HasValue)
            query = query.Where(a => a.JobPositionId == jobPositionId.Value);
        if (stageId.HasValue)
            query = query.Where(a => a.StageId == stageId.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => a.Name.Contains(search) || a.Email.Contains(search));

        var totalCount = await query.CountAsync();

        var applicants = await query
            .OrderByDescending(a => a.AppliedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<ApplicantListDto>
        {
            Items = applicants.Select(MapToListDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<List<ApplicantListDto>> GetApplicantsByStageAsync(Guid? jobPositionId)
    {
        var query = Applicants
            .Include(a => a.JobPosition)
            .Include(a => a.Stage)
            .AsQueryable();

        if (jobPositionId.HasValue)
            query = query.Where(a => a.JobPositionId == jobPositionId.Value);

        var applicants = await query.OrderBy(a => a.Stage.Order).ThenByDescending(a => a.AppliedDate).ToListAsync();
        return applicants.Select(MapToListDto).ToList();
    }

    public async Task<ApplicantDetailDto?> GetApplicantByIdAsync(Guid id)
    {
        var applicant = await Applicants
            .Include(a => a.JobPosition)
            .Include(a => a.Stage)
            .Include(a => a.Activities)
            .FirstOrDefaultAsync(a => a.Id == id);

        return applicant == null ? null : MapToDetailDto(applicant);
    }

    public async Task<ApplicantDetailDto> CreateApplicantAsync(CreateApplicantDto dto)
    {
        var applicant = new Applicant
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            Mobile = dto.Mobile,
            LinkedIn = dto.LinkedIn,
            Degree = dto.Degree,
            JobPositionId = dto.JobPositionId,
            StageId = dto.StageId,
            Tags = dto.Tags ?? [],
            Subject = dto.Subject,
            AppliedDate = DateTime.UtcNow
        };

        Applicants.Add(applicant);
        await context.SaveChangesAsync();

        await context.Entry(applicant).Reference(a => a.JobPosition).LoadAsync();
        await context.Entry(applicant).Reference(a => a.Stage).LoadAsync();
        return MapToDetailDto(applicant);
    }

    public async Task<ApplicantDetailDto?> UpdateApplicantAsync(Guid id, UpdateApplicantDto dto)
    {
        var applicant = await Applicants
            .Include(a => a.JobPosition)
            .Include(a => a.Stage)
            .Include(a => a.Activities)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (applicant == null) return null;

        if (dto.Name != null) applicant.Name = dto.Name;
        if (dto.Email != null) applicant.Email = dto.Email;
        if (dto.Phone != null) applicant.Phone = dto.Phone;
        if (dto.Mobile != null) applicant.Mobile = dto.Mobile;
        if (dto.LinkedIn != null) applicant.LinkedIn = dto.LinkedIn;
        if (dto.Degree != null) applicant.Degree = dto.Degree;
        if (dto.Rating.HasValue) applicant.Rating = dto.Rating.Value;
        if (dto.JobPositionId.HasValue) applicant.JobPositionId = dto.JobPositionId.Value;
        if (dto.StageId.HasValue) applicant.StageId = dto.StageId.Value;
        if (dto.Tags != null) applicant.Tags = dto.Tags;
        if (dto.HasSMS.HasValue) applicant.HasSMS = dto.HasSMS.Value;
        if (dto.ResumeUrl != null) applicant.ResumeUrl = dto.ResumeUrl;
        if (dto.CoverLetterUrl != null) applicant.CoverLetterUrl = dto.CoverLetterUrl;
        if (dto.Notes != null) applicant.Notes = dto.Notes;
        if (dto.Subject != null) applicant.Subject = dto.Subject;

        applicant.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapToDetailDto(applicant);
    }

    public async Task<ApplicantDetailDto?> MoveApplicantToStageAsync(Guid id, MoveApplicantDto dto)
    {
        var applicant = await Applicants
            .Include(a => a.JobPosition)
            .Include(a => a.Stage)
            .Include(a => a.Activities)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (applicant == null) return null;

        applicant.StageId = dto.StageId;
        applicant.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        await context.Entry(applicant).Reference(a => a.Stage).LoadAsync();
        return MapToDetailDto(applicant);
    }

    public async Task<bool> DeleteApplicantAsync(Guid id)
    {
        var applicant = await Applicants.FindAsync(id);
        if (applicant == null) return false;

        Applicants.Remove(applicant);
        await context.SaveChangesAsync();
        return true;
    }

    private static ApplicantListDto MapToListDto(Applicant a) => new(
        a.Id, a.Name, a.Email, a.Phone, a.Mobile, a.LinkedIn, a.Degree,
        a.Rating, a.JobPositionId, a.JobPosition?.Title ?? "",
        a.StageId, a.Stage?.Name ?? "", a.Tags, a.HasSMS, a.AppliedDate
    );

    private static ApplicantDetailDto MapToDetailDto(Applicant a) => new(
        a.Id, a.Name, a.Email, a.Phone, a.Mobile, a.LinkedIn, a.Degree,
        a.Rating, a.JobPositionId, a.JobPosition?.Title ?? "",
        a.StageId, a.Stage?.Name ?? "", a.Tags, a.HasSMS, a.AppliedDate,
        a.ResumeUrl, a.CoverLetterUrl, a.Notes, a.Subject,
        a.Activities.Select(act => new ActivityDto(
            act.Id, act.ApplicantId, act.Type.ToString().ToLower(),
            act.Title, act.DueDate, act.AssignedTo, act.Done)).ToList()
    );
}
