using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;

namespace WSTickets.Api.Controllers;

[ApiController]
[Route("api/tickets/{ticketId}/attachments")]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttachmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttachmentDto>>> GetAttachments(int ticketId)
    {
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var attachments = await _context.Attachments
            .Where(a => a.TicketId == ticketId)
            .Select(a => new AttachmentDto
            {
                Id = a.Id,
                FilePath = baseUrl + a.FilePath,
                FileType = a.FileType,
                UploadedAt = a.UploadedAt,
                UploadedById = a.UploadedById,
                UploadedByName = a.UploadedBy.FullName
            })
            .ToListAsync();

        return Ok(attachments);
    }

    [HttpPost]
    public async Task<ActionResult<AttachmentDto>> UploadAttachment(
    int ticketId,
    [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var ticketExists = await _context.Tickets.AnyAsync(t => t.Id == ticketId);
        if (!ticketExists)
            return NotFound($"Ticket with ID {ticketId} not found.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var fileName = Path.GetFileName(file.FileName);
        var filePathPhysical = Path.Combine(uploadsFolder, fileName);

        // save
        await using (var stream = new FileStream(filePathPhysical, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // create and save entity
        var attachment = new Attachment
        {
            TicketId = ticketId,
            FilePath = $"/uploads/{fileName}",
            FileType = file.ContentType,
            UploadedAt = DateTime.UtcNow,
            UploadedById = userId
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        // project to DTO
        var response = new AttachmentDto
        {
            Id = attachment.Id,
            FilePath = baseUrl + attachment.FilePath,
            FileType = attachment.FileType,
            UploadedAt = attachment.UploadedAt,
            UploadedById = attachment.UploadedById
        };

        return CreatedAtAction(
            nameof(GetAttachments),
            new { ticketId },
            response
        );
    }

}
