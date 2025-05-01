using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WSTickets.App.Services;

public class AuthService
{
    private static readonly Lazy<AuthService> _instance = new(() => new AuthService());
    public static AuthService Instance => _instance.Value;

    private AuthService() { }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var credentials = new { username, password };
        var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

        var response = await ApiClient.Client.PostAsync("auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();

            // Store token securely
            await SecureStorage.SetAsync("auth_token", token);

            // Set Authorization header globally
            ApiClient.SetAuthToken(token);

            return true;
        }

        return false;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await SecureStorage.GetAsync("auth_token");

        if (!string.IsNullOrWhiteSpace(token))
        {
            ApiClient.SetAuthToken(token);
            return true;
        }

        return false;
    }

    public async Task LogoutAsync()
    {
        await SecureStorage.SetAsync("auth_token", string.Empty);
        ApiClient.SetAuthToken(string.Empty); // clear header
    }
}
