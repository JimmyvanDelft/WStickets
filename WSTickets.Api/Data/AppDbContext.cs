using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Net.Sockets;
using ModelsAttachment = WSTickets.Api.Models.Entities.Attachment;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WSTickets.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<StatusHistory> StatusHistories { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ModelsAttachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Reporter)
            .WithMany(u => u.ReportedTickets)
            .HasForeignKey(t => t.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Company)
            .WithMany(c => c.Tickets)
            .HasForeignKey(t => t.CompanyId);

        modelBuilder.Entity<StatusHistory>()
            .HasOne(sh => sh.Ticket)
            .WithMany(t => t.StatusHistories)
            .HasForeignKey(sh => sh.TicketId);

        modelBuilder.Entity<StatusHistory>()
            .HasOne(sh => sh.ChangedBy)
            .WithMany(u => u.StatusChanges)
            .HasForeignKey(sh => sh.ChangedById);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Ticket)
            .WithMany(t => t.Messages)
            .HasForeignKey(m => m.TicketId);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Author)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.AuthorId);

        modelBuilder.Entity<ModelsAttachment>()
            .HasOne(a => a.Ticket)
            .WithMany(t => t.Attachments)
            .HasForeignKey(a => a.TicketId);

        modelBuilder.Entity<ModelsAttachment>()
            .HasOne(a => a.UploadedBy)
            .WithMany(u => u.Attachments)
            .HasForeignKey(a => a.UploadedById);
    }
}
