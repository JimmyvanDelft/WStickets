using System.Text.Json;
using WSTickets.App.Models;

namespace WSTickets.App.Services;

public class TicketService
{
    private static readonly Lazy<TicketService> _instance = new(() => new TicketService());
    public static TicketService Instance => _instance.Value;

    private TicketService() { }

    public async Task<List<Ticket>> GetMyTicketsAsync()
    {
        var response = await ApiClient.Client.GetAsync("tickets/my");

        if (!response.IsSuccessStatusCode)
            return new List<Ticket>();

        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<Ticket>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new();
    }
}
