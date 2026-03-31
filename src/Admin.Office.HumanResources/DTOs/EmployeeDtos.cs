namespace Admin.Office.HumanResources.DTOs;

/// <summary>
/// Single DTO used for both list and detail responses.
/// JSON property names are camelCase by default in ASP.NET Core.
/// </summary>
public class EmployeeDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string? Manager { get; set; }
    public string? Coach { get; set; }
    public string? WorkLocation { get; set; }
    public List<string> Tags { get; set; } = [];
    public string Avatar { get; set; } = string.Empty;
    public string Presence { get; set; } = "offline";
    public string WorkType { get; set; } = "office";
    public List<string> Skills { get; set; } = [];
    public string StartDate { get; set; } = string.Empty;
    public PrivateInfoDto PrivateInfo { get; set; } = new();
    public TimeOffInfoDto TimeOff { get; set; } = new();
    public bool IsPinned { get; set; }
    public ResumeDto? Resume { get; set; }
}

public class PrivateInfoDto
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PersonalEmail { get; set; } = string.Empty;
    public string PersonalPhone { get; set; } = string.Empty;
    public string? BankAccount { get; set; }
}

public class TimeOffInfoDto
{
    public string Status { get; set; } = "none";
    public string? Type { get; set; }
}

public class ResumeDto
{
    public List<WorkExperienceDto> Experience { get; set; } = [];
    public List<EducationDto> Education { get; set; } = [];
}

public class WorkExperienceDto
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string? EndDate { get; set; }
    public string? Description { get; set; }
}

public class EducationDto
{
    public string Degree { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
}

public class CreateEmployeeDto
{
    public string Name { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Manager { get; set; }
    public string? Coach { get; set; }
    public string? WorkLocation { get; set; }
    public List<string>? Tags { get; set; }
    public string? Avatar { get; set; }
    public string? WorkType { get; set; }
    public List<string>? Skills { get; set; }
    public string? StartDate { get; set; }
}

public class UpdateEmployeeDto
{
    public string? Name { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Department { get; set; }
    public string? Manager { get; set; }
    public string? Coach { get; set; }
    public string? WorkLocation { get; set; }
    public List<string>? Tags { get; set; }
    public string? Avatar { get; set; }
    public string? Presence { get; set; }
    public string? WorkType { get; set; }
    public List<string>? Skills { get; set; }
    public string? StartDate { get; set; }
    public PrivateInfoDto? PrivateInfo { get; set; }
    public TimeOffInfoDto? TimeOff { get; set; }
    public bool? IsPinned { get; set; }
}

public class ReportingStatsDto
{
    public int TotalEmployees { get; set; }
    public int OnlineCount { get; set; }
    public int TimeOffCount { get; set; }
    public int DeptCount { get; set; }
    public List<DeptDistributionDto> DeptDistribution { get; set; } = [];
}

public class DeptDistributionDto
{
    public string Department { get; set; } = string.Empty;
    public int Count { get; set; }
}
