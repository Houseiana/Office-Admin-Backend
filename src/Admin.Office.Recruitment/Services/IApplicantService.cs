using Admin.Office.Recruitment.DTOs;
using Admin.Office.Shared.Models;

namespace Admin.Office.Recruitment.Services;

public interface IApplicantService
{
    Task<PagedResponse<ApplicantListDto>> GetApplicantsAsync(
        Guid? jobPositionId = null,
        Guid? stageId = null,
        string? search = null,
        int page = 1,
        int pageSize = 20);
    Task<List<ApplicantListDto>> GetApplicantsByStageAsync(Guid? jobPositionId = null);
    Task<ApplicantDetailDto?> GetApplicantByIdAsync(Guid id);
    Task<ApplicantDetailDto> CreateApplicantAsync(CreateApplicantDto dto);
    Task<ApplicantDetailDto?> UpdateApplicantAsync(Guid id, UpdateApplicantDto dto);
    Task<ApplicantDetailDto?> MoveApplicantToStageAsync(Guid id, MoveApplicantDto dto);
    Task<bool> DeleteApplicantAsync(Guid id);
}
