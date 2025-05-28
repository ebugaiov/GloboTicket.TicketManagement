using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastructure;

public interface ICsvExporter
{
    byte[] ExportEventsToCsv(List<EventExportDto> eventExportDtos);
}
