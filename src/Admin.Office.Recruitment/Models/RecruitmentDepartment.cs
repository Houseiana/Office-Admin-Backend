using Admin.Office.Shared.Models;

namespace Admin.Office.Recruitment.Models;

public class RecruitmentDepartment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<JobPosition> JobPositions { get; set; } = [];
}
