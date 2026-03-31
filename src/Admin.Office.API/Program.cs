using Admin.Office.API.Data;
using Admin.Office.HumanResources;
using Admin.Office.Recruitment;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=AdminOffice.db"));

// Register DbContext as base DbContext for module services
builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<AppDbContext>());

// Register modules
HumanResourcesModule.RegisterServices(builder.Services);
RecruitmentModule.RegisterServices(builder.Services);

// Add controllers from all assemblies
builder.Services.AddControllers()
    .AddApplicationPart(typeof(HumanResourcesModule).Assembly)
    .AddApplicationPart(typeof(RecruitmentModule).Assembly);

builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontends", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:3001",
                "http://localhost:3002")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Auto-migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowFrontends");
app.UseAuthorization();
app.MapControllers();

app.Run();
