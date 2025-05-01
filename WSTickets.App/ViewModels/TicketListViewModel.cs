using System.Collections.ObjectModel;
using System.Windows.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels;

public class TicketListViewModel : BaseViewModel
{
    public ObservableCollection<Ticket> Tickets { get; set; } = new();
    public ICommand RefreshCommand { get; }

    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    public TicketListViewModel()
    {
        RefreshCommand = new Command(async () => await LoadTicketsAsync());
        Task.Run(LoadTicketsAsync);
    }

    private async Task LoadTicketsAsync()
    {
        IsRefreshing = true;

        var tickets = await TicketService.Instance.GetMyTicketsAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Tickets.Clear();
            foreach (var ticket in tickets)
                Tickets.Add(ticket);
        });

        IsRefreshing = false;
    }
}
