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

    [ObservableProperty]
    private TicketPriority selectedPriority;

    [ObservableProperty]
    private TicketStatus selectedStatus;


    public bool IsSendEnabled => !string.IsNullOrWhiteSpace(NewMessageContent);
    public bool CanEditTicket => AuthService.Instance.CurrentUserRole is "Admin" or "Manager" or "Support";



    public List<TicketPriority> PriorityOptions => Enum.GetValues(typeof(TicketPriority)).Cast<TicketPriority>().ToList();
    public List<TicketStatus> StatusOptions => Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList();


    public ObservableCollection<Message> Messages { get; } = new();
    public ObservableCollection<Attachment> Attachments { get; } = new();
    public ObservableCollection<StatusHistory> StatusHistory { get; } = new();

    public TicketDetailViewModel()
    {
        SendMessageCommand = new AsyncRelayCommand(SendMessageAsync, () => IsSendEnabled);
        AddAttachmentCommand = new AsyncRelayCommand(AddAttachmentAsync);
    }

    partial void OnNewMessageContentChanged(string value)
    {
        // re-evaluate button enablement
        SendMessageCommand.NotifyCanExecuteChanged();
    }

    public async Task LoadTicketAsync(int ticketId)
    {
        Ticket = await TicketService.Instance.GetTicketByIdAsync(ticketId);

        SelectedPriority = Ticket.Priority;
        SelectedStatus = Ticket.CurrentStatus;

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
    public IAsyncRelayCommand AddAttachmentCommand { get; }
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
    private async Task AddAttachmentAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select a file to attach"
            });

            if (result == null)
                return;

            // upload via ChatService
            var attached = await ChatService.Instance
                               .AddAttachmentAsync(Ticket.Id, result.FullPath);

            if (attached != null)
            {
                // update the Attachments collection
                Attachments.Add(attached);
            }
            else
            {
                await Application.Current.MainPage
                    .DisplayAlert("Error", "Could not upload attachment.", "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage
                .DisplayAlert("Error", $"Attachment failed: {ex.Message}", "OK");
        }
    }
}
