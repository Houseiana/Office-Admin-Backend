using Admin.Office.Shared.Models;

namespace Admin.Office.Recruitment.Models;

public class JobPosition : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public RecruitmentDepartment Department { get; set; } = null!;
    public string? ResponsiblePerson { get; set; }
    public int ToRecruit { get; set; }
    public List<Applicant> Applicants { get; set; } = [];
}
