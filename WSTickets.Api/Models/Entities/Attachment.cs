using System;

namespace WSTickets.Api.Models.Entities;

public class Attachment
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public DateTime UploadedAt { get; set; }

    // Foreign keys
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public int UploadedById { get; set; }
    public User UploadedBy { get; set; }

}
