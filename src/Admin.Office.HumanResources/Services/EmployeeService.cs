using Admin.Office.HumanResources.DTOs;
using Admin.Office.HumanResources.Models;
using Admin.Office.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.HumanResources.Services;

public class EmployeeService(DbContext context) : IEmployeeService
{
    private DbSet<Employee> Employees => context.Set<Employee>();
    private DbSet<Department> Departments => context.Set<Department>();

    public async Task<PagedResponse<EmployeeListDto>> GetEmployeesAsync(
        string? search, string? department, string? groupBy,
        List<string>? filters, int page = 1, int pageSize = 20)
    {
        var query = Employees.Include(e => e.Department).AsQueryable();

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

        return new PagedResponse<EmployeeListDto>
        {
            Items = employees.Select(MapToListDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<EmployeeDetailDto?> GetEmployeeByIdAsync(Guid id)
    {
        var employee = await Employees
            .Include(e => e.Department)
            .Include(e => e.WorkExperiences)
            .Include(e => e.Educations)
            .FirstOrDefaultAsync(e => e.Id == id);

        return employee == null ? null : MapToDetailDto(employee);
    }

    public async Task<EmployeeDetailDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var employee = new Employee
        {
            Name = dto.Name,
            JobTitle = dto.JobTitle,
            Email = dto.Email,
            Phone = dto.Phone,
            DepartmentId = dto.DepartmentId,
            Manager = dto.Manager,
            Coach = dto.Coach,
            WorkLocation = dto.WorkLocation,
            Tags = dto.Tags ?? [],
            Avatar = dto.Avatar,
            WorkType = Enum.TryParse<WorkType>(dto.WorkType, true, out var wt) ? wt : WorkType.Office,
            Skills = dto.Skills ?? [],
            StartDate = dto.StartDate
        };

        Employees.Add(employee);
        await context.SaveChangesAsync();

        await context.Entry(employee).Reference(e => e.Department).LoadAsync();
        return MapToDetailDto(employee);
    }

    public async Task<EmployeeDetailDto?> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto dto)
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
        if (dto.DepartmentId.HasValue) employee.DepartmentId = dto.DepartmentId;
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
        if (dto.StartDate.HasValue) employee.StartDate = dto.StartDate.Value;
        if (dto.Address != null) employee.Address = dto.Address;
        if (dto.City != null) employee.City = dto.City;
        if (dto.State != null) employee.State = dto.State;
        if (dto.Zip != null) employee.Zip = dto.Zip;
        if (dto.Country != null) employee.Country = dto.Country;
        if (dto.PersonalEmail != null) employee.PersonalEmail = dto.PersonalEmail;
        if (dto.PersonalPhone != null) employee.PersonalPhone = dto.PersonalPhone;
        if (dto.BankAccount != null) employee.BankAccount = dto.BankAccount;
        if (dto.TimeOffStatus != null && Enum.TryParse<TimeOffStatus>(dto.TimeOffStatus, true, out var ts))
            employee.TimeOffStatus = ts;
        if (dto.TimeOffType != null) employee.TimeOffType = dto.TimeOffType;
        if (dto.IsPinned.HasValue) employee.IsPinned = dto.IsPinned.Value;

        employee.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        return MapToDetailDto(employee);
    }

    public async Task<bool> DeleteEmployeeAsync(Guid id)
    {
        var employee = await Employees.FindAsync(id);
        if (employee == null) return false;

        Employees.Remove(employee);
        await context.SaveChangesAsync();
        return true;
    }

    private static EmployeeListDto MapToListDto(Employee e) => new(
        e.Id, e.Name, e.JobTitle, e.Email, e.Phone,
        e.Department?.Name, e.Manager, e.WorkLocation,
        e.Tags, e.Avatar, e.Presence.ToString().ToLower(),
        e.WorkType.ToString().ToLower(), e.Skills, e.StartDate,
        new TimeOffInfoDto(e.TimeOffStatus.ToString().ToLower(), e.TimeOffType),
        e.IsPinned
    );

    private static EmployeeDetailDto MapToDetailDto(Employee e) => new(
        e.Id, e.Name, e.JobTitle, e.Email, e.Phone,
        e.DepartmentId, e.Department?.Name, e.Manager, e.Coach,
        e.WorkLocation, e.Tags, e.Avatar,
        e.Presence.ToString().ToLower(), e.WorkType.ToString().ToLower(),
        e.Skills, e.StartDate,
        new PrivateInfoDto(e.Address, e.City, e.State, e.Zip, e.Country,
            e.PersonalEmail, e.PersonalPhone, e.BankAccount),
        new TimeOffInfoDto(e.TimeOffStatus.ToString().ToLower(), e.TimeOffType),
        e.IsPinned,
        e.WorkExperiences.Select(w => new WorkExperienceDto(
            w.Id, w.Company, w.Position, w.Description, w.StartDate, w.EndDate)).ToList(),
        e.Educations.Select(ed => new EducationDto(
            ed.Id, ed.Institution, ed.Degree, ed.FieldOfStudy, ed.StartDate, ed.EndDate)).ToList()
    );
}
