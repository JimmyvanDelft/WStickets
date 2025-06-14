using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Controllers;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;
using Xunit;

namespace WSTickets.Api.Tests.Controllers;

public class StatusHistoriesControllerTests
{
    private AppDbContext _context;
    private StatusHistoriesController _controller;

    public StatusHistoriesControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("StatusHistoryTestDb_" + System.Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);

        _context.Companies.Add(new Company { Id = 1, Name = "Testbedrijf" });
        _context.Roles.Add(new Role { Id = 2, Name = "Customer" });
        _context.Users.Add(new User
        {
            Id = 1,
            Username = "testuser",
            FullName = "Test Gebruiker",
            Email = "test@example.com",
            PasswordHash = "hash123",
            RoleId = 2,
            CompanyId = 1
        });

        _context.Tickets.Add(new Ticket
        {
            Id = 1,
            Title = "Printer kapot",
            Description = "De printer werkt niet meer.",
            ReporterId = 1,
            CompanyId = 1,
            Priority = TicketPriority.Medium,
            CurrentStatus = TicketStatus.Open
        });

        _context.StatusHistories.Add(new StatusHistory
        {
            Id = 1,
            TicketId = 1,
            Status = TicketStatus.Open,
            Timestamp = System.DateTime.UtcNow,
            ChangedById = 1
        });

        _context.SaveChanges();

        _controller = new StatusHistoriesController(_context);
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
    public async Task GetStatusHistories_ReturnsCorrectHistory()
    {
        // Act
        var result = await _controller.GetStatusHistories(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var history = Assert.IsAssignableFrom<List<StatusHistoryDto>>(okResult.Value);

        Assert.Single(history);
        Assert.Equal(1, history[0].ChangedById);
        Assert.Equal(TicketStatus.Open, history[0].Status);
    }

    [Fact]
    public async Task GetStatusHistories_ReturnsEmptyList_WhenNoneExist()
    {
        var result = await _controller.GetStatusHistories(999); // ticketId not present

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var history = Assert.IsAssignableFrom<List<StatusHistoryDto>>(okResult.Value);

        Assert.Empty(history);
    }
}
