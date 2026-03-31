namespace Admin.Office.Recruitment.DTOs;

public record JobPositionDto(
    Guid Id,
    string Title,
    Guid DepartmentId,
    string DepartmentName,
    string? ResponsiblePerson,
    int NewApplications,
    int ToRecruit,
    int TotalApplications
);

public record CreateJobPositionDto(
    string Title,
    Guid DepartmentId,
    string? ResponsiblePerson,
    int ToRecruit
);

public record UpdateJobPositionDto(
    string? Title,
    Guid? DepartmentId,
    string? ResponsiblePerson,
    int? ToRecruit
);

public record RecruitmentDepartmentDto(
    Guid Id,
    string Name,
    int JobPositionCount,
    List<JobPositionDto> JobPositions
);

public record CreateRecruitmentDepartmentDto(string Name);
public record UpdateRecruitmentDepartmentDto(string? Name);
