using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Services;

public static class ApiClient
{
    private static readonly HttpClient _client = new()
    {
        BaseAddress = new Uri("http://127.0.0.1:5131/api/")
    };

    public static HttpClient Client => _client;

    public static void SetAuthToken(string token)
    {
        _client.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    // Wrapper for automatic 401 handling
    public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        var response = await _client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await AuthService.Instance.LogoutAsync("Your session has expired. Please log in again.");
        }

        return response;
    }
}

