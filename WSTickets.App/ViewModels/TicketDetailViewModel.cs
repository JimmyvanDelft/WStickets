using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
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

    public ObservableCollection<Message> Messages { get; } = new();
    public ObservableCollection<Attachment> Attachments { get; } = new();
    public ObservableCollection<StatusHistory> StatusHistory { get; } = new();

    public async Task LoadTicketAsync(int ticketId)
    {
        Ticket = await TicketService.Instance.GetTicketByIdAsync(ticketId);

        var messages = (await TicketService.Instance.GetMessagesAsync(ticketId))
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
}
