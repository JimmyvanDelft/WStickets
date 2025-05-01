using System;
using System.Collections.Generic;
using System.Linq;
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
