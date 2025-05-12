using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;
using WSTickets.App.Views;

namespace WSTickets.App.ViewModels;

public partial class TicketDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private Ticket ticket;

    [ObservableProperty]
    private string newMessageContent;

    public bool IsSendEnabled => !string.IsNullOrWhiteSpace(NewMessageContent);

    public ObservableCollection<Message> Messages { get; } = new();
    public ObservableCollection<Attachment> Attachments { get; } = new();
    public ObservableCollection<StatusHistory> StatusHistory { get; } = new();

    public TicketDetailViewModel()
    {
        SendMessageCommand = new AsyncRelayCommand(SendMessageAsync, () => IsSendEnabled);
    }

    partial void OnNewMessageContentChanged(string value)
    {
        // re-evaluate button enablement
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    public async Task LoadTicketAsync(int ticketId)
    {
        Ticket = await TicketService.Instance.GetTicketByIdAsync(ticketId);

        var messages = (await ChatService.Instance.GetMessagesAsync(ticketId))
            .OrderBy(m => m.Timestamp)
            .ToList();
        Messages.Clear();
        foreach (var message in messages){
           message.IsFromReporter = (message.AuthorId == Ticket.ReporterId);
           Messages.Add(message);
        }

        var attachments = await TicketService.Instance.GetAttachmentsAsync(ticketId);
        Attachments.Clear();
        foreach (var attachment in attachments)
            Attachments.Add(attachment);

        var statusHistory = await TicketService.Instance.GetStatusHistoryAsync(ticketId);
        StatusHistory.Clear();
        foreach (var status in statusHistory)
            StatusHistory.Add(status);
    }

    public ICommand OpenImageCommand => new Command<string>((filePath) =>
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        var currentPage = Application.Current.MainPage;
        if (currentPage is not null)
        {
            var popup = new ImagePopup(filePath);
            currentPage.ShowPopup(popup);
        }
    });
    public IAsyncRelayCommand SendMessageCommand { get; }
    private async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMessageContent))
            return;

        var sent = await ChatService.Instance.AddMessageAsync(
            Ticket.Id,
            NewMessageContent.Trim(),
            isInternal: false);

        if (sent != null)
        {
            sent.IsFromReporter = sent.AuthorId == Ticket.ReporterId;
            Messages.Add(sent);

            // clear the draft
            NewMessageContent = string.Empty;
        }
        else
        {
            await Application.Current.MainPage
                .DisplayAlert("Error", "Could not send message", "OK");
        }
    }
}
