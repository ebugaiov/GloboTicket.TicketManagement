namespace GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Models.Mail;

public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}