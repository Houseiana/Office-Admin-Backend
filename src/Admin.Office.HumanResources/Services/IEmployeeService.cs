using Admin.Office.HumanResources.DTOs;
using Admin.Office.Shared.Models;

namespace Admin.Office.HumanResources.Services;

public interface IEmployeeService
{
    Task<PagedResponse<EmployeeListDto>> GetEmployeesAsync(
        string? search = null,
        string? department = null,
        string? groupBy = null,
        List<string>? filters = null,
        int page = 1,
        int pageSize = 20);
    Task<EmployeeDetailDto?> GetEmployeeByIdAsync(Guid id);
    Task<EmployeeDetailDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDetailDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(Guid id);
}
