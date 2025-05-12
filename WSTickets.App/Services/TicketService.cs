using System.Net.Http.Json;
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

    public async Task<Ticket?> GetTicketByIdAsync(int ticketId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"tickets/{ticketId}");
            var response = await ApiClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Ticket>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return null;
        }
    }

    //public async Task<List<Message>> GetMessagesAsync(int ticketId)
    //{
    //    var request = new HttpRequestMessage(HttpMethod.Get, $"tickets/{ticketId}/messages");
    //    var response = await ApiClient.SendAsync(request);
    //    var json = await response.Content.ReadAsStringAsync();
    //    return JsonSerializer.Deserialize<List<Message>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
    //}

    public async Task<List<Attachment>> GetAttachmentsAsync(int ticketId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"tickets/{ticketId}/attachments");
        var response = await ApiClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Attachment>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
    }

    public async Task<List<StatusHistory>> GetStatusHistoryAsync(int ticketId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"tickets/{ticketId}/statushistory");
        var response = await ApiClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<StatusHistory>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
    }

    // in WSTickets.App.Services.TicketService.cs
    public async Task<Ticket?> CreateTicketAsync(TicketCreateDto dto)
    {
        try
        {
            // Build the request
            var request = new HttpRequestMessage(HttpMethod.Post, "tickets")
            {
                Content = JsonContent.Create(dto)
            };

            // Send (and get 401 handling too)
            var response = await ApiClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[TicketService] Create failed: {(int)response.StatusCode} – {error}");
                return null;
            }

            // Deserialize into your Ticket model (or a TicketDto if you prefer)
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Ticket>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[TicketService] Exception in CreateTicketAsync: {ex}");
            return null;
        }
    }

}
