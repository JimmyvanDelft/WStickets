using WSTickets.App.ViewModels;
using WSTickets.App.Services;
using WSTickets.App.Models;
using WSTickets.App.Views;

namespace WSTickets.App.Views;

public partial class TicketListPage : ContentPage
{
    private TicketListViewModel _viewModel;

    public TicketListPage()
    {
        InitializeComponent();
        _viewModel = new TicketListViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel != null)
        {
            // Only refresh if not already refreshing
            if (!_viewModel.IsRefreshing)
                _viewModel.RefreshCommand.Execute(null);
        }
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Ticket selectedTicket)
        {
            // Deselect the item
            ((CollectionView)sender).SelectedItem = null;

            // Navigate to detail page with ticket ID
            await Shell.Current.GoToAsync($"{nameof(TicketDetailPage)}?id={selectedTicket.Id}");
        }
    }

    private async void OnNewTicketButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewTicketPage());
    }
}