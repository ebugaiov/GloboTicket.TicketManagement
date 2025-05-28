using Microsoft.AspNetCore.Mvc;
using MediatR;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventList;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;

namespace GloboTicket.TicketManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : Controller
{
    private readonly IMediator _mediator;
    
    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetAllEvents")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EventListVm>>> GetAllEvents()
    {
        var dtos = await _mediator.Send(new GetEventListQuery());
        return Ok(dtos);
    }

    [HttpGet("{id}", Name = "GetEventById")]
    public async Task<ActionResult<EventDetailVm>> GetEventById(Guid id)
    {
        var getEventDetailQuery = new GetEventDetailQuery() { Id = id };
        var dto = await _mediator.Send(getEventDetailQuery);
        return Ok(dto);
    }

    [HttpPost(Name = "AddEvent")]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateEventCommand createEventCommand)
    {
        var id = await _mediator.Send(createEventCommand);
        return Ok(id);
    }

    [HttpPut(Name = "UpdateEvent")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update([FromBody] UpdateEventCommand updateEventCommand)
    {
        await _mediator.Send(updateEventCommand);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteEvent")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id)
    {
        var deleteEventCommand = new DeleteEventCommand() { EventId = id };
        await _mediator.Send(deleteEventCommand);
        return NoContent();
    }

    [HttpGet("export", Name = "ExportEvents")]
    public async Task<FileResult> ExportEvents()
    {
        var fileDto = await _mediator.Send(new GetEventExportQuery());

        return File(fileDto.Data, fileDto.ContentType, fileDto.EventExportFileName);
    }
    
}