using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WSTickets.App.Services;

public class AuthService
{
    private static readonly Lazy<AuthService> _instance = new(() => new AuthService());
    public static AuthService Instance => _instance.Value;

    private readonly HttpClient _httpClient;

    private AuthService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://127.0.0.1:5131/api/")
        };
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        var credentials = new { username, password };
        var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadAsStringAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            // TODO token opslaan in SecureStorage
            return true;
        }

        return false;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await SecureStorage.GetAsync("auth_token");
        return !string.IsNullOrWhiteSpace(token);
    }

}
