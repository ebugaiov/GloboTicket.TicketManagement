using MediatR;
using AutoMapper;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;

public class GetEventExportQueryHandler : IRequestHandler<GetEventExportQuery, EventExportFileVm>
{
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IMapper _mapper;
    private readonly ICsvExporter _csvExporter;
    
    public GetEventExportQueryHandler(IAsyncRepository<Event> eventRepository, IMapper mapper, ICsvExporter csvExporter)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
        _csvExporter = csvExporter;
    }

    public async Task<EventExportFileVm> Handle(GetEventExportQuery request, CancellationToken cancellationToken)
    {
        var allEvents = _mapper.Map<List<EventExportDto>>((await _eventRepository.ListAllAsync()).OrderBy(x => x.Date));
        
        var fileData = _csvExporter.ExportEventsToCsv(allEvents);
        
        var eventExportFileDto = new EventExportFileVm()
        {
            ContentType = "text/csv",
            Data = fileData,
            EventExportFileName = $"{Guid.NewGuid()}.csv"
        };
        
        return eventExportFileDto;
    }
}