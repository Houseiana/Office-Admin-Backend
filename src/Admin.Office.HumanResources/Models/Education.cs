using Admin.Office.Shared.Models;

namespace Admin.Office.HumanResources.Models;

public class Education : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public string Degree { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
}
