namespace WSTickets.Api.Models.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsInternal { get; set; }
    public DateTime Timestamp { get; set; }
    public int AuthorId { get; set; }
}

public class MessageCreateDto
{
    public string Content { get; set; }
    public bool IsInternal { get; set; }
}
