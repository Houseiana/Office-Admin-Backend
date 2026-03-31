using Admin.Office.Shared.Models;

namespace Admin.Office.HumanResources.Models;

public class Employee : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string? Manager { get; set; }
    public string? Coach { get; set; }
    public string? WorkLocation { get; set; }
    public List<string> Tags { get; set; } = [];
    public string? Avatar { get; set; }
    public PresenceStatus Presence { get; set; } = PresenceStatus.Offline;
    public WorkType WorkType { get; set; } = WorkType.Office;
    public List<string> Skills { get; set; } = [];
    public DateTime StartDate { get; set; }

    // Private info
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Country { get; set; }
    public string? PersonalEmail { get; set; }
    public string? PersonalPhone { get; set; }
    public string? BankAccount { get; set; }

    // Time off
    public TimeOffStatus TimeOffStatus { get; set; } = TimeOffStatus.None;
    public string? TimeOffType { get; set; }

    public bool IsPinned { get; set; }

    // Resume
    public List<WorkExperience> WorkExperiences { get; set; } = [];
    public List<Education> Educations { get; set; } = [];
}

public enum PresenceStatus { Online, Offline, Away }
public enum WorkType { Office, Remote, Hybrid }
public enum TimeOffStatus { Approved, Pending, None }
