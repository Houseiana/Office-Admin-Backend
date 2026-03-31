using Admin.Office.HumanResources.DTOs;
using Admin.Office.HumanResources.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.HumanResources.Services;

public class DepartmentService(DbContext context) : IDepartmentService
{
    private DbSet<Department> Departments => context.Set<Department>();

    public async Task<List<DepartmentDto>> GetDepartmentsAsync()
    {
        var departments = await Departments
            .Include(d => d.Children)
            .Include(d => d.Employees)
            .Where(d => d.ParentId == null)
            .ToListAsync();

        return departments.Select(MapToDto).ToList();
    }

    public async Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id)
    {
        var dept = await Departments
            .Include(d => d.Children)
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);

        return dept == null ? null : MapToDto(dept);
    }

    public async Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto dto)
    {
        var dept = new Department
        {
            Name = dto.Name,
            ParentId = dto.ParentId
        };

        Departments.Add(dept);
        await context.SaveChangesAsync();

        return MapToDto(dept);
    }

    public async Task<DepartmentDto?> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto dto)
    {
        var dept = await Departments
            .Include(d => d.Children)
            .Include(d => d.Employees)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dept == null) return null;

        if (dto.Name != null) dept.Name = dto.Name;
        if (dto.ParentId.HasValue) dept.ParentId = dto.ParentId;

        dept.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapToDto(dept);
    }

    public async Task<bool> DeleteDepartmentAsync(Guid id)
    {
        var dept = await Departments.FindAsync(id);
        if (dept == null) return false;

        Departments.Remove(dept);
        await context.SaveChangesAsync();
        return true;
    }

    private static DepartmentDto MapToDto(Department d) => new()
    {
        Id = d.Id.ToString(),
        Name = d.Name,
        ParentId = d.ParentId?.ToString(),
        EmployeeCount = d.Employees.Count,
        Children = d.Children.Count > 0
            ? d.Children.Select(MapToDto).ToList()
            : null
    };
}
