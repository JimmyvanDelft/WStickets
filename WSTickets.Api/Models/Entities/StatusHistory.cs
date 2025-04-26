using System;
using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Models.Entities;

public class StatusHistory
{
    public int Id { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime Timestamp { get; set; }

    // Foreign keys
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public int ChangedById { get; set; }
    public User ChangedBy { get; set; }
}
