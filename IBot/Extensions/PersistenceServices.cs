using IBot.Core.Interfaces.Repositories;
using IBot.DAL.Context;
using IBot.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IBot.Extensions;

public static class PersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}