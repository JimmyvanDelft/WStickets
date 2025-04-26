using System.Collections.Generic;
using System.Net.Sockets;

namespace WSTickets.Api.Models.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}
