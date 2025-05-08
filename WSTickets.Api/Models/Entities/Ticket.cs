using System;
using System.Collections.Generic;
using System.Net.Mail;
using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Models.Entities;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus CurrentStatus { get; set; }

    // Foreign keys
    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public int ReporterId { get; set; }
    public User Reporter { get; set; }

    public int? AssigneeId { get; set; }
    public User Assignee { get; set; }


    public ICollection<StatusHistory> StatusHistories { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
    public List<StatusHistory> StatusHistory { get; set; } = new();
}
