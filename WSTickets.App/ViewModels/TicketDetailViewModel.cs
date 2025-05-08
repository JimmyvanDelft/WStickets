using CommunityToolkit.Mvvm.ComponentModel;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels;

public partial class TicketDetailViewModel : ObservableObject
{
    [ObservableProperty]
    private Ticket ticket;

    public async Task LoadTicketAsync(int ticketId)
    {
        Ticket = await TicketService.Instance.GetTicketByIdAsync(ticketId);
    }
}
