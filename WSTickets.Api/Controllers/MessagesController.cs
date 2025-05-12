using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;

namespace WSTickets.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId}/messages")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MessagesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int ticketId)
    {
        var messages = await _context.Messages
            .Where(m => m.TicketId == ticketId)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                Content = m.Content,
                IsInternal = m.IsInternal,
                Timestamp = m.Timestamp,
                AuthorId = m.AuthorId
            })
            .ToListAsync();

        return Ok(messages);
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> AddMessage(int ticketId, [FromBody] MessageCreateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var message = new Message
        {
            TicketId = ticketId,
            Content = dto.Content,
            IsInternal = dto.IsInternal,
            Timestamp = DateTime.UtcNow,
            AuthorId = userId
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var response = new MessageDto
        {
            Id = message.Id,
            Content = message.Content,
            IsInternal = message.IsInternal,
            Timestamp = message.Timestamp,
            AuthorId = message.AuthorId
        };

        return CreatedAtAction(
            nameof(GetMessages),
            new { ticketId },
            response
        );
    }

}
