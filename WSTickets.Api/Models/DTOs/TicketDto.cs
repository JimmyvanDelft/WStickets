using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Models.DTOs;

public class TicketDto
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

public class TicketUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TicketPriority? Priority { get; set; }
    public TicketStatus? CurrentStatus { get; set; }
}
