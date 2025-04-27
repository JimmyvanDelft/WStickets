using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        var attachments = await _context.Attachments
            .Where(a => a.TicketId == ticketId)
            .Select(a => new AttachmentDto
            {
                Id = a.Id,
                FilePath = a.FilePath,
                FileType = a.FileType,
                UploadedAt = a.UploadedAt,
                UploadedById = a.UploadedById
            })
            .ToListAsync();

        return Ok(attachments);
    }

    [HttpPost]
    public async Task<IActionResult> UploadAttachment(int ticketId, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var filePath = Path.Combine(uploadsFolder, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        var attachment = new Attachment
        {
            TicketId = ticketId,
            FilePath = $"/uploads/{file.FileName}",
            FileType = file.ContentType,
            UploadedAt = DateTime.UtcNow,
            UploadedById = userId
        };

        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAttachments), new { ticketId = ticketId }, null);
    }
}
