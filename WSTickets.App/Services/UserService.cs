using System.Net.Http.Json;
using System.Text.Json;
using WSTickets.App.Models;

namespace WSTickets.App.Services;

public class UserService
{
    private static readonly Lazy<UserService> _instance = new(() => new UserService());
    public static UserService Instance => _instance.Value;

    private UserService() { }

    public async Task<List<User>> GetUsersAsync()
    {
        try
        {
            var response = await ApiClient.Client.GetAsync("users");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[UserService] Error: {ex.Message}");
            return new();
        }
    }

    public async Task<bool> CreateUserAsync(UserCreateDto dto)
    {
        try
        {
            var response = await ApiClient.Client.PostAsJsonAsync("users", dto);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        try
        {
            var response = await ApiClient.Client.DeleteAsync($"users/{userId}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }


}
