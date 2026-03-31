using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Admin.Office.Shared.Interfaces;

public interface IModuleRegistration
{
    static abstract void RegisterServices(IServiceCollection services);
    static abstract void ConfigureDatabase(ModelBuilder modelBuilder);
}
