namespace Admin.Office.Recruitment.DTOs;

public record ActivityDto(
    Guid Id,
    Guid ApplicantId,
    string Type,
    string Title,
    DateTime DueDate,
    string? AssignedTo,
    bool Done
);

public record CreateActivityDto(
    Guid ApplicantId,
    string Type,
    string Title,
    DateTime DueDate,
    string? AssignedTo
);

public record UpdateActivityDto(
    string? Type,
    string? Title,
    DateTime? DueDate,
    string? AssignedTo,
    bool? Done
);
