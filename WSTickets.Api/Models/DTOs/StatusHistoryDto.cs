using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Models.DTOs;

public class StatusHistoryDto
{
    public int Id { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime Timestamp { get; set; }
    public int ChangedById { get; set; }
}

public class StatusHistoryCreateDto
{
    public TicketStatus Status { get; set; }
}
