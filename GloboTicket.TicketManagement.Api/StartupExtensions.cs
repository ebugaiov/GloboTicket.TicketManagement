using Microsoft.EntityFrameworkCore;
using GloboTicket.TicketManagement.Persistence;
using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Api.Middleware;

namespace GloboTicket.TicketManagement.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddPersistenceServices(builder.Configuration);
        
        builder.Services.AddControllers();

        builder.Services.AddCors(options => options.AddPolicy(
            "open",
            policy =>
                policy.WithOrigins(
                    [
                        builder.Configuration["ApiUrl"] ?? "https://localhost:5001",
                        builder.Configuration["BlazorUrl"] ?? "https://localhost:5002"
                    ])
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(pol => true)
                    .AllowAnyHeader()
                    .AllowCredentials()));

        builder.Services.AddSwaggerGen();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors("open");

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseCustomExceptionHandler();
        
        app.UseHttpsRedirection();
        app.MapControllers();

        return app;
    }

    public static async Task ResetDabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        try
        {
            var context = scope.ServiceProvider.GetService<GloboTicketDbContext>();
            if (context != null)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            // add logging here later on
        }
    }
}
