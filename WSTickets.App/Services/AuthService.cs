using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WSTickets.App.Models;

namespace WSTickets.App.Services;

public class AuthService
{
    private static readonly Lazy<AuthService> _instance = new(() => new AuthService());
    public static AuthService Instance => _instance.Value;

    private AuthService() { }

    public async Task<(bool Success, string ErrorMessage)> LoginAsync(string username, string password)
    {
        var credentials = new { username, password };
        var response = await ApiClient.Client.PostAsJsonAsync("auth/login", credentials);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return (false, "Incorrect username or password.");
            var error = await response.Content.ReadAsStringAsync();
            return (false, $"Server error {response.StatusCode}: {error}");
        }

        AuthResponse result;
        try
        {
            result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (result == null || string.IsNullOrWhiteSpace(result.Token))
                return (false, "Login succeeded but no token returned.");
        }
        catch (JsonException)
        {
            return (false, "Could not parse authentication response.");
        }

        try
        {
            await SecureStorage.SetAsync("auth_token", result.Token);
        }
        catch (Exception ex)
        {
            return (false, $"Failed to save token: {ex.Message}");
        }

        ApiClient.SetAuthToken(result.Token);

        return (true, string.Empty);
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

    public async Task LogoutAsync(string? message = null)
    {
        SecureStorage.Remove("auth_token");
        ApiClient.SetAuthToken("");

        // Navigate to login page
        await Shell.Current.GoToAsync("//LoginPage");

        if (!string.IsNullOrWhiteSpace(message))
        {
            await Shell.Current.DisplayAlert("Logged Out", message, "OK");
        }
    }

}
