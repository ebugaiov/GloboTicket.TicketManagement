namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

public class EventDetailVm
{
    public Guid EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
    public string Artist { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryDto Category { get; set; } = default!;
}