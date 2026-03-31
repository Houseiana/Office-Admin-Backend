using Admin.Office.Shared.Models;

namespace Admin.Office.HumanResources.Models;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public Department? Parent { get; set; }
    public List<Department> Children { get; set; } = [];
    public List<Employee> Employees { get; set; } = [];
}
