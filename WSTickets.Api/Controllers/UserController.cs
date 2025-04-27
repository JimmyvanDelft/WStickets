using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;
using BCrypt.Net;

namespace WSTickets.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.Company)
            .Include(u => u.Role)
            .Select(u => new UserReadDto
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email,
                CompanyName = u.Company.Name,
                RoleName = u.Role.Name
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserReadDto>> GetUser(int id)
    {
        var user = await _context.Users
            .Include(u => u.Company)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        var userDto = new UserReadDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            CompanyName = user.Company.Name,
            RoleName = user.Role.Name
        };

        return Ok(userDto);
    }

    // POST: api/users
    [HttpPost]
    public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userDto)
    {
        var newUser = new User
        {
            Username = userDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            FullName = userDto.FullName,
            Email = userDto.Email,
            CompanyId = userDto.CompanyId,
            RoleId = userDto.RoleId
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var createdUserDto = new UserReadDto
        {
            Id = newUser.Id,
            Username = newUser.Username,
            FullName = newUser.FullName,
            Email = newUser.Email,
            CompanyName = (await _context.Companies.FindAsync(newUser.CompanyId))?.Name,
            RoleName = (await _context.Roles.FindAsync(newUser.RoleId))?.Name
        };

        return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, createdUserDto);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userDto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        user.FullName = userDto.FullName ?? user.FullName;
        user.Email = userDto.Email ?? user.Email;
        if (userDto.CompanyId.HasValue)
            user.CompanyId = userDto.CompanyId.Value;
        if (userDto.RoleId.HasValue)
            user.RoleId = userDto.RoleId.Value;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
