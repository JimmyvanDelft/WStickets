namespace WSTickets.App.Models;

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Urgent
}
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
}
