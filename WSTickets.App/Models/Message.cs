using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace WSTickets.App.Models;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsInternal { get; set; }
    public DateTime Timestamp { get; set; }
    public int AuthorId { get; set; }

    [JsonIgnore]
    public bool IsFromReporter { get; set; }
}
