using System.Text.Json;
using System.Net.Http.Json;
using WSTickets.App.Models;

namespace WSTickets.App.Services;

public class CompanyService
{
    private static readonly Lazy<CompanyService> _instance = new(() => new CompanyService());
    public static CompanyService Instance => _instance.Value;

    private CompanyService() { }

    public async Task<List<CompanyDto>> GetCompaniesAsync()
    {
        var response = await ApiClient.Client.GetAsync("companies");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CompanyDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new();
    }

    public async Task<CompanyDto?> CreateCompanyAsync(string name)
    {
        var dto = new { Name = name };
        var response = await ApiClient.Client.PostAsJsonAsync("companies", dto);

        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CompanyDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
