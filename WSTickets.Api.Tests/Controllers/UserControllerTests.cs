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

public class UsersControllerTests
{
    private AppDbContext _context;
    private UsersController _controller;

    public UsersControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("UsersTestDb_" + System.Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);

        _context.Companies.Add(new Company { Id = 1, Name = "Testbedrijf" });
        _context.Roles.Add(new Role { Id = 1, Name = "Admin" });

        _context.Users.Add(new User
        {
            Id = 1,
            Username = "admin",
            FullName = "Admin User",
            Email = "admin@example.com",
            PasswordHash = "hash",
            CompanyId = 1,
            RoleId = 1
        });

        _context.SaveChanges();

        _controller = new UsersController(_context);
        SetUserContext(userId: 1, role: "Admin");
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
    public async Task GetUsers_ReturnsAllUsers()
    {
        var result = await _controller.GetUsers();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var users = Assert.IsAssignableFrom<List<UserReadDto>>(okResult.Value);
        Assert.Single(users);
        Assert.Equal("admin", users[0].Username);
    }

    [Fact]
    public async Task GetUser_ReturnsCorrectUser()
    {
        var result = await _controller.GetUser(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var user = Assert.IsType<UserReadDto>(okResult.Value);
        Assert.Equal(1, user.Id);
    }

    [Fact]
    public async Task CreateUser_CreatesNewUser()
    {
        var dto = new UserCreateDto
        {
            Username = "newuser",
            FullName = "New User",
            Email = "new@example.com",
            Password = "secret",
            CompanyId = 1,
            RoleId = 1
        };

        var result = await _controller.CreateUser(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdUser = Assert.IsType<UserReadDto>(createdResult.Value);
        Assert.Equal("newuser", createdUser.Username);
    }

    [Fact]
    public async Task UpdateUser_ChangesValues()
    {
        var dto = new UserUpdateDto
        {
            FullName = "Updated Name",
            Email = "updated@example.com",
            CompanyId = 1,
            RoleId = 1
        };

        var result = await _controller.UpdateUser(1, dto);

        Assert.IsType<NoContentResult>(result);

        var user = await _context.Users.FindAsync(1);
        Assert.Equal("Updated Name", user.FullName);
        Assert.Equal("updated@example.com", user.Email);
    }

    [Fact]
    public async Task DeleteUser_RemovesUser()
    {
        var result = await _controller.DeleteUser(1);

        Assert.IsType<NoContentResult>(result);

        var user = await _context.Users.FindAsync(1);
        Assert.Null(user);
    }
}
