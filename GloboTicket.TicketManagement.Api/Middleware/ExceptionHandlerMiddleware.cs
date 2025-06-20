using System.Net;
using System.Text.Json;
using GloboTicket.TicketManagement.Application.Exceptions;

namespace GloboTicket.TicketManagement.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ConvertException(context, ex);
        }
    }

    private Task ConvertException(HttpContext context, Exception excepton)
    {
        HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
        
        var result = string.Empty;

        switch (excepton)
        {
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.ValidationErrors);
                break;
            case BadRequestException badRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = badRequestException.Message;
                break;
            case NotFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case Exception:
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
        }
        
        context.Response.StatusCode = (int)httpStatusCode;
        
        if (result == string.Empty)
            result = JsonSerializer.Serialize(new { error = excepton.Message });

        return context.Response.WriteAsync(result);
    }
}