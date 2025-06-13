using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;
using WSTickets.App.Views;

namespace WSTickets.App.ViewModels;

public partial class AllTicketsViewModel : TicketListViewModel
{
    public AllTicketsViewModel() : base()
    {
        RefreshCommand = new AsyncRelayCommand(LoadTicketsAsync);
        Task.Run(LoadTicketsAsync);
    }

    public IAsyncRelayCommand RefreshCommand { get; protected set; }

    protected override async Task LoadTicketsAsync()
    {
        IsRefreshing = true;

        try
        {
            var tickets = await TicketService.Instance.GetAllTicketsAsync();
            _allTickets = tickets;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                ApplyFiltersAndSort();
                HasError = false;
            });
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong fetching all tickets: {ex.Message}";
            HasError = true;
        }
        finally
        {
            IsRefreshing = false;
        }
    }
}
