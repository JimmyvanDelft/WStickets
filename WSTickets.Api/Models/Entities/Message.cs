namespace WSTickets.Api.Models.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsInternal { get; set; }
    public DateTime Timestamp { get; set; }

    // Foreign keys
    public int TicketId { get; set; }
    public Ticket Ticket { get; set; }

    public int AuthorId { get; set; }
    public User Author { get; set; }

}
