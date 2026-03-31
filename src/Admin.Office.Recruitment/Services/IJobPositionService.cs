using Admin.Office.Recruitment.DTOs;

namespace Admin.Office.Recruitment.Services;

public interface IJobPositionService
{
    Task<List<RecruitmentDepartmentDto>> GetDepartmentsWithPositionsAsync(Guid? departmentId = null);
    Task<JobPositionDto?> GetJobPositionByIdAsync(Guid id);
    Task<JobPositionDto> CreateJobPositionAsync(CreateJobPositionDto dto);
    Task<JobPositionDto?> UpdateJobPositionAsync(Guid id, UpdateJobPositionDto dto);
    Task<bool> DeleteJobPositionAsync(Guid id);

    Task<List<RecruitmentDepartmentDto>> GetRecruitmentDepartmentsAsync();
    Task<RecruitmentDepartmentDto> CreateRecruitmentDepartmentAsync(CreateRecruitmentDepartmentDto dto);
    Task<RecruitmentDepartmentDto?> UpdateRecruitmentDepartmentAsync(Guid id, UpdateRecruitmentDepartmentDto dto);
    Task<bool> DeleteRecruitmentDepartmentAsync(Guid id);
}
