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
using Xunit;

namespace WSTickets.Api.Tests.Controllers;

public class MessagesControllerTests
{
    private AppDbContext _context;
    private MessagesController _controller;

    public MessagesControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("MessagesTestDb_" + System.Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);

        _context.Companies.Add(new Company { Id = 1, Name = "Testbedrijf" });
        _context.Roles.Add(new Role { Id = 2, Name = "Customer" });

        _context.Users.Add(new User
        {
            Id = 1,
            Username = "testuser",
            FullName = "Test Gebruiker",
            PasswordHash = "hash123",
            RoleId = 2,
            CompanyId = 1,
            Email = "test@example.com"
        });

        _context.Tickets.Add(new Ticket
        {
            Id = 1,
            Title = "Printer kapot",
            Description = "Ja hij rookt zelfs",
            ReporterId = 1,
            CompanyId = 1,
            CurrentStatus = Models.Enums.TicketStatus.Open
        });

        _context.Messages.Add(new Message
        {
            Id = 1,
            TicketId = 1,
            Content = "Testbericht",
            AuthorId = 1,
            Timestamp = System.DateTime.UtcNow,
            IsInternal = false
        });

        _context.SaveChanges();

        _controller = new MessagesController(_context);
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
    public async Task GetMessages_ShouldReturnMessagesForTicket()
    {
        var result = await _controller.GetMessages(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var messages = Assert.IsAssignableFrom<List<MessageDto>>(okResult.Value);

        Assert.Single(messages);
        Assert.Equal("Testbericht", messages[0].Content);
    }

    [Fact]
    public async Task AddMessage_ShouldAddAndReturnMessage()
    {
        var createDto = new MessageCreateDto
        {
            Content = "Nieuw bericht",
            IsInternal = true
        };

        var result = await _controller.AddMessage(1, createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var message = Assert.IsType<MessageDto>(createdResult.Value);

        Assert.Equal("Nieuw bericht", message.Content);
        Assert.True(message.IsInternal);
        Assert.Equal(2, message.Id);
    }
}
