
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Controllers;
using WSTickets.Api.Data;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;
using WSTickets.Api.Models.DTOs;
using Xunit;

namespace WSTickets.Api.Tests.Controllers;

public class TicketsControllerTests_Extended
{
    private AppDbContext _context;
    private TicketsController _controller;

    public TicketsControllerTests_Extended()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("ExtendedTestDb_" + System.Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);

        _context.Companies.Add(new Company { Id = 1, Name = "Testbedrijf" });
        _context.Users.Add(new User
        {
            Id = 1,
            Username = "testuser",
            FullName = "Test Gebruiker",
            PasswordHash = "hash123",
            Role = new Role { Id = 2, Name = "Customer" },
            Email = "test@example.com",
            CompanyId = 1
        });
        _context.Users.Add(new User
        {
            Id = 2,
            Username = "supportuser",
            FullName = "Support Gebruiker",
            PasswordHash = "hash456",
            Role = new Role { Id = 3, Name = "Support" },
            Email = "support@example.com",
            CompanyId = 1
        });
        _context.SaveChanges();

        _controller = new TicketsController(_context);
        SetUserContext(userId: 1, role: "Customer");
    }

    private void SetUserContext(int userId, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    [Fact]
    public async Task CreateTicket_ShouldReturnCreatedTicket()
    {
        var dto = new TicketCreateDto
        {
            Title = "Nieuw ticket",
            Description = "Beschrijving",
            Priority = TicketPriority.High,
            CompanyId = 1
        };

        var result = await _controller.CreateTicket(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTicket = Assert.IsType<TicketDto>(createdResult.Value);
        Assert.Equal("Nieuw ticket", createdTicket.Title);
        Assert.Equal(TicketPriority.High, createdTicket.Priority);
    }

    [Fact]
    public async Task UpdateTicket_ShouldChangeFields()
    {
        // Arrange
        _context.Tickets.Add(new Ticket
        {
            Id = 5,
            Title = "Originele titel",
            Description = "Oude beschrijving",
            ReporterId = 1,
            CompanyId = 1,
            Priority = TicketPriority.Medium,
            CurrentStatus = TicketStatus.Open
        });
        _context.SaveChanges();

        var dto = new TicketUpdateDto
        {
            Title = "Aangepast",
            Description = "Nieuwe beschrijving",
            Priority = TicketPriority.Low,
            CurrentStatus = TicketStatus.Closed
        };

        // Act
        var result = await _controller.UpdateTicket(5, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var updatedTicket = await _context.Tickets.FindAsync(5);
        Assert.Equal("Aangepast", updatedTicket.Title);
        Assert.Equal(TicketStatus.Closed, updatedTicket.CurrentStatus);
    }

    [Fact]
    public async Task DeleteTicket_ShouldRemoveTicket()
    {
        // Arrange
        _context.Tickets.Add(new Ticket
        {
            Id = 99,
            Title = "Verwijder mij",
            Description = "Dit ticket moet verwijderd worden",
            ReporterId = 1,
            CompanyId = 1,
            Priority = TicketPriority.Medium,
            CurrentStatus = TicketStatus.Open
        });
        _context.SaveChanges();

        // Act
        var result = await _controller.DeleteTicket(99);

        // Assert
        Assert.IsType<NoContentResult>(result);
        var deleted = await _context.Tickets.FindAsync(99);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task AssignTicket_ShouldSetAssignee()
    {
        _context.Tickets.Add(new Ticket
        {
            Id = 100,
            Title = "Assign test",
            Description = "Test description",
            ReporterId = 1,
            CompanyId = 1,
            Priority = TicketPriority.Medium,
            CurrentStatus = TicketStatus.Open
        });
        _context.SaveChanges();

        var result = await _controller.AssignTicket(100, 2);

        Assert.IsType<NoContentResult>(result);
        var ticket = await _context.Tickets.FindAsync(100);
        Assert.Equal(2, ticket.AssigneeId);
    }
}
