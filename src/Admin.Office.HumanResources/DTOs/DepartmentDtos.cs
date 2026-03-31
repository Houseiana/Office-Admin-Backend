namespace Admin.Office.HumanResources.DTOs;

public record DepartmentDto(
    Guid Id,
    string Name,
    Guid? ParentId,
    int EmployeeCount,
    List<DepartmentDto> Children
);

public record CreateDepartmentDto(string Name, Guid? ParentId);
public record UpdateDepartmentDto(string? Name, Guid? ParentId);
