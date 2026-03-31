using Admin.Office.HumanResources.Models;
using Admin.Office.HumanResources.Services;
using Admin.Office.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Office.HumanResources;

public class HumanResourcesModule : IModuleRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
    }

    public static void ConfigureDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("HR_Departments");
            entity.HasMany(d => d.Children).WithOne(d => d.Parent).HasForeignKey(d => d.ParentId);
            entity.HasMany(d => d.Employees).WithOne(e => e.Department).HasForeignKey(e => e.DepartmentId);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("HR_Employees");
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Tags).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            entity.Property(e => e.Skills).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });

        modelBuilder.Entity<WorkExperience>(entity =>
        {
            entity.ToTable("HR_WorkExperiences");
            entity.HasOne(w => w.Employee).WithMany(e => e.WorkExperiences).HasForeignKey(w => w.EmployeeId);
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.ToTable("HR_Educations");
            entity.HasOne(ed => ed.Employee).WithMany(e => e.Educations).HasForeignKey(ed => ed.EmployeeId);
        });
    }
}
