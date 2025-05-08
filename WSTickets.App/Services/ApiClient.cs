using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

namespace WSTickets.App.Services;

public static class ApiClient
{
    private static readonly HttpClient _client;

    static ApiClient()
    {
        string baseAddress;

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            // Android emulator moet 10.0.2.2 gebruiken om host machine te bereiken
            baseAddress = "http://10.0.2.2:5131/api/";
        }
        else
        {
            // Windows, iOS simulator, etc.
            baseAddress = "http://localhost:5131/api/";
        }

        _client = new HttpClient
        {
            BaseAddress = new Uri(baseAddress)
        };
    }


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

