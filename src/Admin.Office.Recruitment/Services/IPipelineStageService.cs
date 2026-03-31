using Admin.Office.Recruitment.DTOs;

namespace Admin.Office.Recruitment.Services;

public interface IPipelineStageService
{
    Task<List<PipelineStageDto>> GetStagesAsync();
    Task<PipelineStageDto?> GetStageByIdAsync(Guid id);
    Task<PipelineStageDto> CreateStageAsync(CreatePipelineStageDto dto);
    Task<PipelineStageDto?> UpdateStageAsync(Guid id, UpdatePipelineStageDto dto);
    Task<bool> DeleteStageAsync(Guid id);
}
