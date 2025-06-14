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

        var success = await LoadCurrentUserAsync();
        if (!success)
            return (false, "Failed to load user info after login.");

        return (true, string.Empty);
    }

    public string? CurrentUserRole { get; internal set; }
    public int? CurrentUserId { get; private set; }

    public async Task<bool> LoadCurrentUserAsync()
    {
        try
        {
            var response = await ApiClient.Client.GetAsync("auth/me");
            if (!response.IsSuccessStatusCode)
                return false;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<UserInfoDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data is null) return false;

            CurrentUserId = data.UserId;
            CurrentUserRole = data.Role;

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await SecureStorage.GetAsync("auth_token");

        if (!string.IsNullOrWhiteSpace(token))
        {
            ApiClient.SetAuthToken(token);
            return true;
        }

        LoadCurrentUserAsync();

        return false;
    }

    public async Task LogoutAsync(string? message = null)
    {
        SecureStorage.Remove("auth_token");
        ApiClient.SetAuthToken("");

        App.NavigateToLoginPage();

        if (!string.IsNullOrWhiteSpace(message))
        {
            await Application.Current.MainPage.DisplayAlert("Logged Out", message, "OK");
        }
    }

}
