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

}
