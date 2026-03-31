using Admin.Office.HumanResources;
using Admin.Office.Recruitment;
using Microsoft.EntityFrameworkCore;

namespace Admin.Office.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        HumanResourcesModule.ConfigureDatabase(modelBuilder);
        RecruitmentModule.ConfigureDatabase(modelBuilder);
    }
}
