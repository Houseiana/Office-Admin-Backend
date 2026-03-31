using Admin.Office.Shared.Models;

namespace Admin.Office.Recruitment.Models;

public class Applicant : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? LinkedIn { get; set; }
    public string? Degree { get; set; }
    public int Rating { get; set; } // 0-5
    public Guid JobPositionId { get; set; }
    public JobPosition JobPosition { get; set; } = null!;
    public Guid StageId { get; set; }
    public PipelineStage Stage { get; set; } = null!;
    public List<string> Tags { get; set; } = [];
    public bool HasSMS { get; set; }
    public DateTime AppliedDate { get; set; }
    public string? ResumeUrl { get; set; }
    public string? CoverLetterUrl { get; set; }
    public string? Notes { get; set; }
    public string? Subject { get; set; }
    public List<Activity> Activities { get; set; } = [];
}
