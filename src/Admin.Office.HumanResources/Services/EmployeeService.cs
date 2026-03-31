using Admin.Office.HumanResources.DTOs;
using Admin.Office.HumanResources.Models;
using Admin.Office.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.HumanResources.Services;

public class EmployeeService(DbContext context) : IEmployeeService
{
    private DbSet<Employee> Employees => context.Set<Employee>();
    private DbSet<Department> Departments => context.Set<Department>();

    public async Task<PagedResponse<EmployeeDto>> GetEmployeesAsync(
        string? search, string? department, string? groupBy,
        List<string>? filters, int page = 1, int pageSize = 20)
    {
        var query = Employees
            .Include(e => e.Department)
            .Include(e => e.WorkExperiences)
            .Include(e => e.Educations)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(e =>
                e.Name.Contains(search) ||
                e.JobTitle.Contains(search) ||
                e.Email.Contains(search));

        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(e => e.Department != null && e.Department.Name == department);

        if (filters != null)
        {
            foreach (var filter in filters)
            {
                query = filter switch
                {
                    "at_work" => query.Where(e => e.Presence == PresenceStatus.Online),
                    "time_off" => query.Where(e => e.TimeOffStatus == TimeOffStatus.Approved),
                    "newly_hired" => query.Where(e => e.StartDate >= DateTime.UtcNow.AddMonths(-6)),
                    "remote" => query.Where(e => e.WorkType == WorkType.Remote || e.WorkType == WorkType.Hybrid),
                    _ => query
                };
            }
        }

        var totalCount = await query.CountAsync();

        var employees = await query
            .OrderBy(e => e.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResponse<EmployeeDto>
        {
            Items = employees.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await Employees
            .Include(e => e.Department)
            .Include(e => e.WorkExperiences)
            .Include(e => e.Educations)
            .FirstOrDefaultAsync(e => e.Id == id);

        return employee == null ? null : MapToDto(employee);
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        Guid? departmentId = null;
        if (!string.IsNullOrWhiteSpace(dto.Department))
        {
            var dept = await Departments.FirstOrDefaultAsync(d => d.Name == dto.Department);
            departmentId = dept?.Id;
        }

        var employee = new Employee
        {
            Name = dto.Name,
            JobTitle = dto.JobTitle,
            Email = dto.Email,
            Phone = dto.Phone,
            DepartmentId = departmentId,
            Manager = dto.Manager,
            Coach = dto.Coach,
            WorkLocation = dto.WorkLocation,
            Tags = dto.Tags ?? [],
            Avatar = dto.Avatar,
            WorkType = Enum.TryParse<WorkType>(dto.WorkType, true, out var wt) ? wt : WorkType.Office,
            Skills = dto.Skills ?? [],
            StartDate = DateTime.TryParse(dto.StartDate, out var sd) ? sd : DateTime.UtcNow
        };

        Employees.Add(employee);
        await context.SaveChangesAsync();

        await context.Entry(employee).Reference(e => e.Department).LoadAsync();
        return MapToDto(employee);
    }

    public async Task<EmployeeDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto)
    {
        var employee = await Employees
            .Include(e => e.Department)
            .Include(e => e.WorkExperiences)
            .Include(e => e.Educations)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null) return null;

        if (dto.Name != null) employee.Name = dto.Name;
        if (dto.JobTitle != null) employee.JobTitle = dto.JobTitle;
        if (dto.Email != null) employee.Email = dto.Email;
        if (dto.Phone != null) employee.Phone = dto.Phone;

        if (dto.Department != null)
        {
            var dept = await Departments.FirstOrDefaultAsync(d => d.Name == dto.Department);
            employee.DepartmentId = dept?.Id;
        }

        if (dto.Manager != null) employee.Manager = dto.Manager;
        if (dto.Coach != null) employee.Coach = dto.Coach;
        if (dto.WorkLocation != null) employee.WorkLocation = dto.WorkLocation;
        if (dto.Tags != null) employee.Tags = dto.Tags;
        if (dto.Avatar != null) employee.Avatar = dto.Avatar;
        if (dto.Presence != null && Enum.TryParse<PresenceStatus>(dto.Presence, true, out var ps))
            employee.Presence = ps;
        if (dto.WorkType != null && Enum.TryParse<WorkType>(dto.WorkType, true, out var wt))
            employee.WorkType = wt;
        if (dto.Skills != null) employee.Skills = dto.Skills;
        if (dto.StartDate != null && DateTime.TryParse(dto.StartDate, out var sd))
            employee.StartDate = sd;

        if (dto.PrivateInfo != null)
        {
            employee.Address = dto.PrivateInfo.Address;
            employee.City = dto.PrivateInfo.City;
            employee.State = dto.PrivateInfo.State;
            employee.Zip = dto.PrivateInfo.Zip;
            employee.Country = dto.PrivateInfo.Country;
            employee.PersonalEmail = dto.PrivateInfo.PersonalEmail;
            employee.PersonalPhone = dto.PrivateInfo.PersonalPhone;
            employee.BankAccount = dto.PrivateInfo.BankAccount;
        }

        if (dto.TimeOff != null)
        {
            if (Enum.TryParse<TimeOffStatus>(dto.TimeOff.Status, true, out var ts))
                employee.TimeOffStatus = ts;
            employee.TimeOffType = dto.TimeOff.Type;
        }

        if (dto.IsPinned.HasValue) employee.IsPinned = dto.IsPinned.Value;

        employee.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapToDto(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await Employees.FindAsync(id);
        if (employee == null) return false;

        Employees.Remove(employee);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ReportingStatsDto> GetReportingStatsAsync()
    {
        var employees = await Employees
            .Include(e => e.Department)
            .ToListAsync();

        var deptDistribution = employees
            .GroupBy(e => e.Department?.Name ?? "Unassigned")
            .Select(g => new DeptDistributionDto
            {
                Department = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(d => d.Count)
            .ToList();

        return new ReportingStatsDto
        {
            TotalEmployees = employees.Count,
            OnlineCount = employees.Count(e => e.Presence == PresenceStatus.Online),
            TimeOffCount = employees.Count(e => e.TimeOffStatus == TimeOffStatus.Approved),
            DeptCount = await Departments.CountAsync(),
            DeptDistribution = deptDistribution
        };
    }

    private static EmployeeDto MapToDto(Employee e)
    {
        var dto = new EmployeeDto
        {
            Id = e.Id.ToString(),
            Name = e.Name,
            JobTitle = e.JobTitle,
            Email = e.Email,
            Phone = e.Phone,
            Department = e.Department?.Name ?? string.Empty,
            Manager = e.Manager,
            Coach = e.Coach,
            WorkLocation = e.WorkLocation,
            Tags = e.Tags,
            Avatar = e.Avatar ?? string.Empty,
            Presence = e.Presence.ToString().ToLower(),
            WorkType = e.WorkType.ToString().ToLower(),
            Skills = e.Skills,
            StartDate = e.StartDate.ToString("yyyy-MM-dd"),
            PrivateInfo = new PrivateInfoDto
            {
                Address = e.Address ?? string.Empty,
                City = e.City ?? string.Empty,
                State = e.State ?? string.Empty,
                Zip = e.Zip ?? string.Empty,
                Country = e.Country ?? string.Empty,
                PersonalEmail = e.PersonalEmail ?? string.Empty,
                PersonalPhone = e.PersonalPhone ?? string.Empty,
                BankAccount = e.BankAccount
            },
            TimeOff = new TimeOffInfoDto
            {
                Status = e.TimeOffStatus.ToString().ToLower(),
                Type = e.TimeOffType
            },
            IsPinned = e.IsPinned,
            Resume = new ResumeDto
            {
                Experience = e.WorkExperiences.Select(w => new WorkExperienceDto
                {
                    Title = w.Title,
                    Company = w.Company,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }).ToList(),
                Education = e.Educations.Select(ed => new EducationDto
                {
                    Degree = ed.Degree,
                    School = ed.School,
                    Year = ed.Year
                }).ToList()
            }
        };

        return dto;
    }
}
