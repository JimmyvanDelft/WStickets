using System.Collections.ObjectModel;
using System.Windows.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels;

public class TicketListViewModel : BaseViewModel
{
    public ObservableCollection<Ticket> Tickets { get; set; } = new();
    public ICommand RefreshCommand { get; }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    private bool _hasError;
    public bool HasError
    {
        get => _hasError;
        set { _hasError = value; OnPropertyChanged(); }
    }


    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set { isRefreshing = value; OnPropertyChanged(); }
    }

    private bool noTicketsMessageVisible;
    public bool TicketsMessageVisible => !noTicketsMessageVisible;
    public bool NoTicketsMessageVisible
    {
        get => noTicketsMessageVisible;
        set { noTicketsMessageVisible = value; OnPropertyChanged(); }
    }


    public TicketListViewModel()
    {
        RefreshCommand = new Command(async () => await LoadTicketsAsync());
        Task.Run(LoadTicketsAsync);
    }

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
