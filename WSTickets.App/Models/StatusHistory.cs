using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Models;

public class StatusHistory
{
    public int Id { get; set; }
    public string Status { get; set; }
    public DateTime Timestamp { get; set; }
    public int ChangedById { get; set; }
}
