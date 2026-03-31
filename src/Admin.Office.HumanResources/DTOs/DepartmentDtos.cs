namespace Admin.Office.HumanResources.DTOs;

public class DepartmentDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ParentId { get; set; }
    public int EmployeeCount { get; set; }
    public List<DepartmentDto>? Children { get; set; }
}

public record CreateDepartmentDto(string Name, Guid? ParentId);
public record UpdateDepartmentDto(string? Name, Guid? ParentId);
