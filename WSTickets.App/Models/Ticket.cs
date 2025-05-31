using System.Text.Json.Serialization;

namespace WSTickets.App.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketPriority
{
    Low,
    Medium,
    High,
    Urgent
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TicketStatus { 
    Open, 
    InProgress, 
    WorkAround, 
    Resolved, 
    Closed 
}

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketPriority Priority { get; set; }
    public TicketStatus CurrentStatus { get; set; }
    public int ReporterId { get; set; }
    public string? ReporterName { get; set; } 
    public int? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
}

public class TicketCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TicketPriority Priority { get; set; }
    public int CompanyId { get; set; }
}

public static class TicketEnums
{
    public static List<TicketPriority> Priorities => Enum.GetValues(typeof(TicketPriority)).Cast<TicketPriority>().ToList();
    public static List<TicketStatus> Statuses => Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList();
}
