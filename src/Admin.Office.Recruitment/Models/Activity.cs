using Admin.Office.Shared.Models;

namespace Admin.Office.Recruitment.Models;

public class Activity : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public Applicant Applicant { get; set; } = null!;
    public ActivityType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public bool Done { get; set; }
}

public enum ActivityType { Call, Interview, Email, Task }
