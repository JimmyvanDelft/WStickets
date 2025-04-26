using Microsoft.EntityFrameworkCore;
namespace WSTickets.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options) { }
    public DbSet<Ticket> Tickets { get; set; }
}
public class Ticket
{
    public int Id { get; set; }
    public string Subject { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}