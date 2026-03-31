using Admin.Office.HumanResources.DTOs;
using Admin.Office.Shared.Models;

namespace Admin.Office.HumanResources.Services;

public interface IEmployeeService
{
    Task<PagedResponse<EmployeeDto>> GetEmployeesAsync(
        string? search = null,
        string? department = null,
        string? groupBy = null,
        List<string>? filters = null,
        int page = 1,
        int pageSize = 20);
    Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto);
    Task<bool> DeleteEmployeeAsync(Guid id);
    Task<ReportingStatsDto> GetReportingStatsAsync();
}
