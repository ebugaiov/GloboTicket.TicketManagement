using GloboTicket.TicketManagement.Api;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

await app.ResetDabaseAsync();

app.Run();
