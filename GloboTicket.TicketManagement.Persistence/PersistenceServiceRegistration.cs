using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Persistence.Repositories;

namespace GloboTicket.TicketManagement.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<GloboTicketDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("GloboTicketTicketManagementConnectionString")));
        
        services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
        
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}