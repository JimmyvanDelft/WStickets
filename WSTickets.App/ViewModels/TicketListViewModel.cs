using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels;

public partial class TicketListViewModel : ObservableObject
{
    public ObservableCollection<Ticket> Tickets { get; } = new();

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool noTicketsMessageVisible;

    public bool TicketsMessageVisible => !NoTicketsMessageVisible;

    public TicketListViewModel()
    {
        RefreshCommand = new AsyncRelayCommand(LoadTicketsAsync);
        Task.Run(LoadTicketsAsync);
    }

    public IAsyncRelayCommand RefreshCommand { get; }

    private async Task LoadTicketsAsync()
    {
        IsRefreshing = true;

        try
        {
            var tickets = await TicketService.Instance.GetMyTicketsAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Tickets.Clear();
                foreach (var ticket in tickets)
                    Tickets.Add(ticket);

                NoTicketsMessageVisible = Tickets.Count == 0;
                HasError = false;
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong fetching your tickets: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}
