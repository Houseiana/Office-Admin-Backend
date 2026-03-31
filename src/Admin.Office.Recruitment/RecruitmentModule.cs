using Admin.Office.Recruitment.Models;
using Admin.Office.Recruitment.Services;
using Admin.Office.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Office.Recruitment;

public class RecruitmentModule : IModuleRegistration
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IJobPositionService, JobPositionService>();
        services.AddScoped<IApplicantService, ApplicantService>();
        services.AddScoped<IPipelineStageService, PipelineStageService>();
        services.AddScoped<IReportingService, ReportingService>();
    }

    public static void ConfigureDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RecruitmentDepartment>(entity =>
        {
            entity.ToTable("REC_Departments");
        });

        modelBuilder.Entity<JobPosition>(entity =>
        {
            entity.ToTable("REC_JobPositions");
            entity.HasOne(j => j.Department).WithMany(d => d.JobPositions).HasForeignKey(j => j.DepartmentId);
        });

        modelBuilder.Entity<PipelineStage>(entity =>
        {
            entity.ToTable("REC_PipelineStages");
            entity.HasIndex(s => s.Order);
        });

        modelBuilder.Entity<Applicant>(entity =>
        {
            entity.ToTable("REC_Applicants");
            entity.HasOne(a => a.JobPosition).WithMany(j => j.Applicants).HasForeignKey(a => a.JobPositionId);
            entity.HasOne(a => a.Stage).WithMany(s => s.Applicants).HasForeignKey(a => a.StageId);
            entity.Property(a => a.Tags).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("REC_Activities");
            entity.HasOne(a => a.Applicant).WithMany(app => app.Activities).HasForeignKey(a => a.ApplicantId);
        });
    }
}
