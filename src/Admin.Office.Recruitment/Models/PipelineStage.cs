using Admin.Office.Shared.Models;

namespace Admin.Office.Recruitment.Models;

public class PipelineStage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool FoldedInKanban { get; set; }
    public bool IsHiredStage { get; set; }
    public string? EmailTemplateId { get; set; }
    public bool JobSpecific { get; set; }
    public string StatusCategory { get; set; } = "in_progress"; // in_progress, blocked, done
    public List<Applicant> Applicants { get; set; } = [];
}
