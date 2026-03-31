using Admin.Office.Recruitment.DTOs;
using Admin.Office.Recruitment.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.Recruitment.Services;

public class PipelineStageService(DbContext context) : IPipelineStageService
{
    private DbSet<PipelineStage> Stages => context.Set<PipelineStage>();

    public async Task<List<PipelineStageDto>> GetStagesAsync()
    {
        var stages = await Stages
            .Include(s => s.Applicants)
            .OrderBy(s => s.Order)
            .ToListAsync();

        return stages.Select(MapToDto).ToList();
    }

    public async Task<PipelineStageDto?> GetStageByIdAsync(Guid id)
    {
        var stage = await Stages.Include(s => s.Applicants).FirstOrDefaultAsync(s => s.Id == id);
        return stage == null ? null : MapToDto(stage);
    }

    public async Task<PipelineStageDto> CreateStageAsync(CreatePipelineStageDto dto)
    {
        var stage = new PipelineStage
        {
            Name = dto.Name,
            Order = dto.Order,
            FoldedInKanban = dto.FoldedInKanban,
            IsHiredStage = dto.IsHiredStage,
            EmailTemplateId = dto.EmailTemplateId,
            JobSpecific = dto.JobSpecific,
            StatusCategory = dto.StatusCategory
        };

        Stages.Add(stage);
        await context.SaveChangesAsync();
        return MapToDto(stage);
    }

    public async Task<PipelineStageDto?> UpdateStageAsync(Guid id, UpdatePipelineStageDto dto)
    {
        var stage = await Stages.Include(s => s.Applicants).FirstOrDefaultAsync(s => s.Id == id);
        if (stage == null) return null;

        if (dto.Name != null) stage.Name = dto.Name;
        if (dto.Order.HasValue) stage.Order = dto.Order.Value;
        if (dto.FoldedInKanban.HasValue) stage.FoldedInKanban = dto.FoldedInKanban.Value;
        if (dto.IsHiredStage.HasValue) stage.IsHiredStage = dto.IsHiredStage.Value;
        if (dto.EmailTemplateId != null) stage.EmailTemplateId = dto.EmailTemplateId;
        if (dto.JobSpecific.HasValue) stage.JobSpecific = dto.JobSpecific.Value;
        if (dto.StatusCategory != null) stage.StatusCategory = dto.StatusCategory;

        stage.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapToDto(stage);
    }

    public async Task<bool> DeleteStageAsync(Guid id)
    {
        var stage = await Stages.FindAsync(id);
        if (stage == null) return false;

        Stages.Remove(stage);
        await context.SaveChangesAsync();
        return true;
    }

    private static PipelineStageDto MapToDto(PipelineStage s) => new(
        s.Id, s.Name, s.Order, s.FoldedInKanban, s.IsHiredStage,
        s.EmailTemplateId, s.JobSpecific, s.StatusCategory,
        s.Applicants.Count
    );
}
