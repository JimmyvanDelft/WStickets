using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;
using WSTickets.App.Views;

namespace WSTickets.App.ViewModels;

public partial class TicketListViewModel : ObservableObject
{
    protected List<Ticket> _allTickets = new();

    public ObservableCollection<Ticket> Tickets { get; } = new();

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private bool hasError;

    [ObservableProperty]
    private bool isRefreshing;

    [ObservableProperty]
    private bool noTicketsMessageVisible;

    [ObservableProperty]
    private bool showActiveOnly = true; // Default to showing only active tickets

    [ObservableProperty]
    private string selectedStatusString = "All Statuses";

    [ObservableProperty]
    private string selectedPriorityString = "All Priorities";

    [ObservableProperty]
    private string selectedSortOption = "Created Date (Newest)";

    [ObservableProperty]
    private string searchQuery = string.Empty;

    public bool TicketsMessageVisible => !NoTicketsMessageVisible;

    // Filter and sort options
    public ObservableCollection<string> StatusFilterOptions { get; }
    public ObservableCollection<string> PriorityFilterOptions { get; }
    public ObservableCollection<string> SortOptions { get; }

    public TicketListViewModel()
    {
        RefreshCommand = new AsyncRelayCommand(LoadTicketsAsync);
        GoToTicketCommand = new AsyncRelayCommand<Ticket>(GoToTicketAsync);
        ApplyFiltersCommand = new RelayCommand(ApplyFiltersAndSort);
        ClearFiltersCommand = new RelayCommand(ClearFilters);

        // Initialize filter options
        StatusFilterOptions = new ObservableCollection<string>
        {
            "All Statuses"
        };
        foreach (var status in Enum.GetValues<TicketStatus>())
        {
            StatusFilterOptions.Add(status.ToString());
        }

        PriorityFilterOptions = new ObservableCollection<string>
        {
            "All Priorities"
        };
        foreach (var priority in Enum.GetValues<TicketPriority>())
        {
            PriorityFilterOptions.Add(priority.ToString());
        }

        SortOptions = new ObservableCollection<string>
        {
            "Created Date (Newest)",
            "Created Date (Oldest)",
            "Priority (High to Low)",
            "Priority (Low to High)",
            "Status",
            "Title (A-Z)",
            "Title (Z-A)"
        };

        Task.Run(LoadTicketsAsync);

        // Watch for filter changes
        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(ShowActiveOnly)
                or nameof(SelectedStatusString)
                or nameof(SelectedPriorityString)
                or nameof(SelectedSortOption)
                or nameof(SearchQuery))
            {
                ApplyFiltersAndSort();
            }
        };
    }

    public IAsyncRelayCommand<Ticket> GoToTicketCommand { get; }
    public IAsyncRelayCommand RefreshCommand { get; }
    public IRelayCommand ApplyFiltersCommand { get; }
    public IRelayCommand ClearFiltersCommand { get; }

    private async Task GoToTicketAsync(Ticket ticket)
    {
        if (ticket is null) return;
        await Shell.Current.GoToAsync($"{nameof(TicketDetailPage)}?id={ticket.Id}");
    }

    protected virtual async Task LoadTicketsAsync()
    {
        IsRefreshing = true;

        try
        {
            var tickets = await TicketService.Instance.GetMyTicketsAsync();
            _allTickets = tickets;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                ApplyFiltersAndSort();
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

    protected void ApplyFiltersAndSort()
    {
        var filteredTickets = _allTickets.AsEnumerable();

        // Apply active filter
        if (ShowActiveOnly)
        {
            // Assuming closed tickets have status "Closed", "Resolved", "Cancelled" etc.
            // Adjust these status values based on your TicketStatus enum
            var inactiveStatuses = new[] { TicketStatus.Closed, TicketStatus.Resolved };
            filteredTickets = filteredTickets.Where(t => !inactiveStatuses.Contains(t.CurrentStatus));
        }

        if (SelectedStatusString != "All Statuses"
          && Enum.TryParse<TicketStatus>(SelectedStatusString, out var st))
        {
            filteredTickets = filteredTickets.Where(t => t.CurrentStatus == st);
        }

        if (SelectedPriorityString != "All Priorities"
          && Enum.TryParse<TicketPriority>(SelectedPriorityString, out var pr))
        {
            filteredTickets = filteredTickets.Where(t => t.Priority == pr);
        }

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            var lowerQuery = SearchQuery.ToLower();

            filteredTickets = filteredTickets.Where(t =>
                (!string.IsNullOrEmpty(t.Title) && t.Title.ToLower().Contains(lowerQuery)) ||
                (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(lowerQuery)) ||
                (!string.IsNullOrEmpty(t.CompanyName) && t.CompanyName.ToLower().Contains(lowerQuery)) ||
                (!string.IsNullOrEmpty(t.AssigneeName) && t.AssigneeName.ToLower().Contains(lowerQuery)) ||
                (!string.IsNullOrEmpty(t.ReporterName) && t.ReporterName.ToLower().Contains(lowerQuery))
            );
        }

        // Apply sorting
        filteredTickets = SelectedSortOption switch
        {
            "Created Date (Newest)" => filteredTickets.OrderByDescending(t => t.Id),
            "Created Date (Oldest)" => filteredTickets.OrderBy(t => t.Id),
            "Priority (High to Low)" => filteredTickets.OrderByDescending(t => (int)t.Priority),
            "Priority (Low to High)" => filteredTickets.OrderBy(t => (int)t.Priority),
            "Status" => filteredTickets.OrderBy(t => t.CurrentStatus.ToString()),
            "Title (A-Z)" => filteredTickets.OrderBy(t => t.Title),
            "Title (Z-A)" => filteredTickets.OrderByDescending(t => t.Title),
            _ => filteredTickets.OrderByDescending(t => t.Id)
        };

        var resultList = filteredTickets.ToList();

        Tickets.Clear();
        foreach (var ticket in resultList)
        {
            Tickets.Add(ticket);
        }

        NoTicketsMessageVisible = Tickets.Count == 0;
    }

    private void ClearFilters()
    {
        ShowActiveOnly = false;
        SelectedStatusString = "All Statuses";
        SelectedPriorityString = "All Priorities";
        SelectedSortOption = "Created Date (Newest)";
        ApplyFiltersAndSort();
    }

}