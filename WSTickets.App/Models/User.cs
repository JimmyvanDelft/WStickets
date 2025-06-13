using System.Text.Json.Serialization;

namespace WSTickets.App.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    [JsonIgnore]
    public bool IsCustomer => RoleName?.ToLower() == "customer";
}

public class UserCreateDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int CompanyId { get; set; }
    public int RoleId { get; set; }
}