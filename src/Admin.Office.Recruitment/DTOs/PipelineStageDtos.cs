namespace Admin.Office.Recruitment.DTOs;

public record PipelineStageDto(
    Guid Id,
    string Name,
    int Order,
    bool FoldedInKanban,
    bool IsHiredStage,
    string? EmailTemplateId,
    bool JobSpecific,
    string StatusCategory,
    int ApplicantCount
);

public record CreatePipelineStageDto(
    string Name,
    int Order,
    bool FoldedInKanban,
    bool IsHiredStage,
    string? EmailTemplateId,
    bool JobSpecific,
    string StatusCategory
);

public record UpdatePipelineStageDto(
    string? Name,
    int? Order,
    bool? FoldedInKanban,
    bool? IsHiredStage,
    string? EmailTemplateId,
    bool? JobSpecific,
    string? StatusCategory
);
