using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Net.Sockets;

namespace WSTickets.Api.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }

    // Foreign keys
    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }


    public ICollection<Ticket> ReportedTickets { get; set; }
    public ICollection<Ticket> AssignedTickets { get; set; }
    public ICollection<Message> Messages { get; set; }
    public ICollection<StatusHistory> StatusChanges { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
}
