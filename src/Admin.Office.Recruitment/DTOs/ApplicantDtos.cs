namespace Admin.Office.Recruitment.DTOs;

public record ApplicantListDto(
    Guid Id,
    string Name,
    string Email,
    string? Phone,
    string? Mobile,
    string? LinkedIn,
    string? Degree,
    int Rating,
    Guid JobPositionId,
    string JobPositionTitle,
    Guid StageId,
    string StageName,
    List<string> Tags,
    bool HasSMS,
    DateTime AppliedDate
);

public record ApplicantDetailDto(
    Guid Id,
    string Name,
    string Email,
    string? Phone,
    string? Mobile,
    string? LinkedIn,
    string? Degree,
    int Rating,
    Guid JobPositionId,
    string JobPositionTitle,
    Guid StageId,
    string StageName,
    List<string> Tags,
    bool HasSMS,
    DateTime AppliedDate,
    string? ResumeUrl,
    string? CoverLetterUrl,
    string? Notes,
    string? Subject,
    List<ActivityDto> Activities
);

public record CreateApplicantDto(
    string Name,
    string Email,
    string? Phone,
    string? Mobile,
    string? LinkedIn,
    string? Degree,
    Guid JobPositionId,
    Guid StageId,
    List<string>? Tags,
    string? Subject
);

public record UpdateApplicantDto(
    string? Name,
    string? Email,
    string? Phone,
    string? Mobile,
    string? LinkedIn,
    string? Degree,
    int? Rating,
    Guid? JobPositionId,
    Guid? StageId,
    List<string>? Tags,
    bool? HasSMS,
    string? ResumeUrl,
    string? CoverLetterUrl,
    string? Notes,
    string? Subject
);

public record MoveApplicantDto(Guid StageId);
