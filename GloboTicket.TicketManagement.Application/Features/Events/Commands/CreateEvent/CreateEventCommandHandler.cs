using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using MediatR;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper, IEmailService emailService)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
        _emailService = emailService;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var @event = _mapper.Map<Event>(request);
        
        var validator = new CreateEventCommandValidator(_eventRepository);
        var validationResult = await validator.ValidateAsync(request);
        
        if (validationResult.Errors.Count > 0)
            throw new Exceptions.ValidationException(validationResult);
        
        @event = await _eventRepository.AddAsync(@event);

        var email = new Email()
        {
            To = "gill@snowball.be",
            Body = $"A new event was created: {request}",
            Subject = "A new event was created"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            // this shouldn't stop the API from doing else so this can be logged
        }

        return @event.EventId;
    }
}