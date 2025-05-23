﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WSTickets.App.Models;

namespace WSTickets.App.Services;

public class ChatService
{
    private static readonly Lazy<ChatService> _instance = new(() => new ChatService());
    public static ChatService Instance => _instance.Value;

    private ChatService() { }

    public async Task<Message?> AddMessageAsync(int ticketId, string content, bool isInternal = false)
    {
        try
        {
            var payload = new { content, isInternal };

            var request = new HttpRequestMessage(HttpMethod.Post, $"tickets/{ticketId}/messages")
            {
                Content = JsonContent.Create(payload)
            };

            var response = await ApiClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[ChatService] AddMessage failed: {response.StatusCode} – {error}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Message>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ChatService] Exception in AddMessageAsync: {ex}");
            return null;
        }
    }

    public async Task<List<Message>> GetMessagesAsync(int ticketId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"tickets/{ticketId}/messages");
            var response = await ApiClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return new List<Message>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Message>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Message>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ChatService] Exception in GetMessagesAsync: {ex}");
            return new List<Message>();
        }
    }

    public async Task<Attachment?> AddAttachmentAsync(int ticketId, string filePath)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            var stream = File.OpenRead(filePath);
            var fileName = Path.GetFileName(filePath);
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, "file", fileName);

            var request = new HttpRequestMessage(HttpMethod.Post, $"tickets/{ticketId}/attachments")
            {
                Content = content
            };

            var response = await ApiClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[ChatService] AddAttachment failed: {response.StatusCode} – {error}");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Attachment>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ChatService] Exception in AddAttachmentAsync: {ex}");
            return null;
        }
    }
}
