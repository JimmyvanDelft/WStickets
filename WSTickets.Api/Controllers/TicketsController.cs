using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TicketsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets([FromQuery] TicketPriority? priority, [FromQuery] TicketStatus? status)
    {
        var query = _context.Tickets.AsQueryable();

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(t => t.CurrentStatus == status.Value);
        }

        var tickets = await query
            .Select(t => new TicketDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Priority = t.Priority,
                CurrentStatus = t.CurrentStatus,
                ReporterId = t.ReporterId,
                AssigneeId = t.AssigneeId,
                CompanyId = t.CompanyId
            })
            .ToListAsync();

        return Ok(tickets);
    }

    [HttpGet("mine")]
    public async Task<ActionResult<IEnumerable<Ticket>>> GetMyTickets()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        int userId = int.Parse(userIdClaim.Value);

        var tickets = await _context.Tickets
            .Where(t => t.ReporterId == userId || t.AssigneeId == userId)
            .ToListAsync();

        return Ok(tickets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketDto>> GetTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
            return NotFound();

        var ticketDto = new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority,
            CurrentStatus = ticket.CurrentStatus,
            ReporterId = ticket.ReporterId,
            AssigneeId = ticket.AssigneeId,
            CompanyId = ticket.CompanyId
        };

        return Ok(ticketDto);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> CreateTicket(TicketCreateDto dto)
    {
        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var ticket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            CurrentStatus = TicketStatus.Open,
            CompanyId = dto.CompanyId,
            ReporterId = userId
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        // Add initial status history
        var statusHistory = new StatusHistory
        {
            TicketId = ticket.Id,
            Status = ticket.CurrentStatus,
            Timestamp = DateTime.UtcNow,
            ChangedById = userId
        };

        _context.StatusHistories.Add(statusHistory);
        await _context.SaveChangesAsync();

        var createdTicketDto = new TicketDto
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority,
            CurrentStatus = ticket.CurrentStatus,
            ReporterId = ticket.ReporterId,
            AssigneeId = ticket.AssigneeId,
            CompanyId = ticket.CompanyId
        };

        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, createdTicketDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(int id, TicketUpdateDto dto)
    {
        var ticket = await _context.Tickets.FindAsync(id);
        if (ticket == null)
            return NotFound();

        bool statusChanged = false;

        if (dto.Title != null)
            ticket.Title = dto.Title;

        if (dto.Description != null)
            ticket.Description = dto.Description;

        if (dto.Priority.HasValue)
            ticket.Priority = dto.Priority.Value;

        if (dto.CurrentStatus.HasValue && ticket.CurrentStatus != dto.CurrentStatus.Value)
        {
            statusChanged = true;
            ticket.CurrentStatus = dto.CurrentStatus.Value;
        }

        await _context.SaveChangesAsync();

        if (statusChanged)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var statusHistory = new StatusHistory
            {
                TicketId = ticket.Id,
                Status = ticket.CurrentStatus,
                Timestamp = DateTime.UtcNow,
                ChangedById = userId
            };

            _context.StatusHistories.Add(statusHistory);
            await _context.SaveChangesAsync();
        }

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
            return NotFound();

        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{id}/assign/{userId}")]
    public async Task<IActionResult> AssignTicket(int id, int userId)
    {
        var ticket = await _context.Tickets.FindAsync(id);

        if (ticket == null)
            return NotFound();

        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            return NotFound(new { message = "User not found" });

        ticket.AssigneeId = userId;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
