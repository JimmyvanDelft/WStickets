using WSTickets.App.ViewModels;

namespace WSTickets.App.Views;

public partial class AllTicketsPage : ContentPage
{
    private AllTicketsViewModel _viewModel;

    public AllTicketsPage()
    {
        InitializeComponent();
        _viewModel = new AllTicketsViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsRefreshing)
            _viewModel.RefreshCommand.Execute(null);
    }

    private async void OnNewTicketButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewTicketPage());
    }
}
