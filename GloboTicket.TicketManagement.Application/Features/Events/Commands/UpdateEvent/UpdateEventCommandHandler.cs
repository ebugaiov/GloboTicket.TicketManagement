using MediatR;
using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Application.Exceptions;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand>
{
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly IMapper _mapper;
    
    public UpdateEventCommandHandler(IAsyncRepository<Event> eventRepository, IMapper mapper)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
    }

    public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var eventToUpdate = await _eventRepository.GetByIdAsync(request.EventId);

        if (eventToUpdate == null)
        {
            throw new NotFoundException(nameof(Event), request.EventId);
        }
        
        var validator = new UpdateEventCommandValidator();
        var validationResult = await validator.ValidateAsync(request);
        
        if (validationResult.Errors.Count > 0)
            throw new ValidationException(validationResult);

        _mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));
        
        await _eventRepository.UpdateAsync(eventToUpdate);
    }
}