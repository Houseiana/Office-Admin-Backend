using Admin.Office.HumanResources.DTOs;

namespace Admin.Office.HumanResources.Services;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetDepartmentsAsync();
    Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id);
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto);
    Task<DepartmentDto?> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto dto);
    Task<bool> DeleteDepartmentAsync(Guid id);
}
