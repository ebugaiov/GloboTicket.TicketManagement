namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;

public class EventExportFileVm
{
    public string EventExportFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[]? Data { get; set; }
}