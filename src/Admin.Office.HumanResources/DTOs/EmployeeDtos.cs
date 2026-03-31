using Admin.Office.HumanResources.Models;

namespace Admin.Office.HumanResources.DTOs;

public record EmployeeListDto(
    Guid Id,
    string Name,
    string JobTitle,
    string Email,
    string Phone,
    string? DepartmentName,
    string? Manager,
    string? WorkLocation,
    List<string> Tags,
    string? Avatar,
    string Presence,
    string WorkType,
    List<string> Skills,
    DateTime StartDate,
    TimeOffInfoDto TimeOff,
    bool IsPinned
);

public record EmployeeDetailDto(
    Guid Id,
    string Name,
    string JobTitle,
    string Email,
    string Phone,
    Guid? DepartmentId,
    string? DepartmentName,
    string? Manager,
    string? Coach,
    string? WorkLocation,
    List<string> Tags,
    string? Avatar,
    string Presence,
    string WorkType,
    List<string> Skills,
    DateTime StartDate,
    PrivateInfoDto PrivateInfo,
    TimeOffInfoDto TimeOff,
    bool IsPinned,
    List<WorkExperienceDto> Experience,
    List<EducationDto> Education
);

public record CreateEmployeeDto(
    string Name,
    string JobTitle,
    string Email,
    string Phone,
    Guid? DepartmentId,
    string? Manager,
    string? Coach,
    string? WorkLocation,
    List<string>? Tags,
    string? Avatar,
    string? WorkType,
    List<string>? Skills,
    DateTime StartDate
);

public record UpdateEmployeeDto(
    string? Name,
    string? JobTitle,
    string? Email,
    string? Phone,
    Guid? DepartmentId,
    string? Manager,
    string? Coach,
    string? WorkLocation,
    List<string>? Tags,
    string? Avatar,
    string? Presence,
    string? WorkType,
    List<string>? Skills,
    DateTime? StartDate,
    string? Address,
    string? City,
    string? State,
    string? Zip,
    string? Country,
    string? PersonalEmail,
    string? PersonalPhone,
    string? BankAccount,
    string? TimeOffStatus,
    string? TimeOffType,
    bool? IsPinned
);

public record PrivateInfoDto(
    string? Address,
    string? City,
    string? State,
    string? Zip,
    string? Country,
    string? PersonalEmail,
    string? PersonalPhone,
    string? BankAccount
);

public record TimeOffInfoDto(string Status, string? Type);

public record WorkExperienceDto(
    Guid Id,
    string Company,
    string Position,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate
);

public record EducationDto(
    Guid Id,
    string Institution,
    string Degree,
    string? FieldOfStudy,
    DateTime StartDate,
    DateTime? EndDate
);
