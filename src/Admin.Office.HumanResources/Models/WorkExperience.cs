using Admin.Office.Shared.Models;

namespace Admin.Office.HumanResources.Models;

public class WorkExperience : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string? EndDate { get; set; }
}
