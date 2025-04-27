using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId}/statushistory")]
[Authorize]
public class StatusHistoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public StatusHistoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusHistoryDto>>> GetStatusHistories(int ticketId)
    {
        var history = await _context.StatusHistories
            .Where(sh => sh.TicketId == ticketId)
            .Select(sh => new StatusHistoryDto
            {
                Id = sh.Id,
                Status = sh.Status,
                Timestamp = sh.Timestamp,
                ChangedById = sh.ChangedById
            })
            .ToListAsync();

        return Ok(history);
    }

    [HttpPost]
    public async Task<IActionResult> AddStatusHistory(int ticketId, [FromBody] StatusHistoryCreateDto dto)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var statusHistory = new StatusHistory
        {
            TicketId = ticketId,
            Status = dto.Status,
            Timestamp = DateTime.UtcNow,
            ChangedById = userId
        };

        _context.StatusHistories.Add(statusHistory);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStatusHistories), new { ticketId = ticketId }, null);
    }
}
