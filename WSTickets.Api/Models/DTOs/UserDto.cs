namespace WSTickets.Api.Models.DTOs;

public class UserCreateDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public int RoleId { get; set; }
}

public class UserReadDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string RoleName { get; set; }
}

public class UserUpdateDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int? CompanyId { get; set; }
    public int? RoleId { get; set; }
}