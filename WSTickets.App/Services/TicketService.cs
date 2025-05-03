using System.Text.Json;
using System.Text.Json.Serialization;
using WSTickets.App.Models;

namespace WSTickets.App.Services;

public class TicketService
{
    private static readonly Lazy<TicketService> _instance = new(() => new TicketService());
    public static TicketService Instance => _instance.Value;

    private TicketService() { }

    public async Task<List<Ticket>> GetMyTicketsAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "tickets/mine");
            var response = await ApiClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[Tickets] Something went wrong: {response.StatusCode} - {error}");
                return new List<Ticket>();
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<Ticket>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Tickets] Exception: {ex.Message}");
            return new List<Ticket>();
        }
    }

}
