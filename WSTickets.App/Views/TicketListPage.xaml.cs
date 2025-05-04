using WSTickets.App.ViewModels;
using WSTickets.App.Services;

namespace WSTickets.App.Views;

public partial class TicketListPage : ContentPage
{
    public TicketListPage()
    {
        InitializeComponent();
        BindingContext = new TicketListViewModel();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await AuthService.Instance.LogoutAsync();
        await Shell.Current.GoToAsync("//LoginPage");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is TicketListViewModel vm)
        {
            // Only refresh if not already refreshing
            if (!vm.IsRefreshing)
                vm.RefreshCommand.Execute(null);
        }
    }

}
