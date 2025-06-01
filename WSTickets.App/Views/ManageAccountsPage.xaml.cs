using WSTickets.App.ViewModels;

namespace WSTickets.App.Views;

public partial class ManageAccountsPage : ContentPage
{
    public ManageAccountsPage()
    {
        InitializeComponent();
        BindingContext = new ManageAccountsViewModel();
    }
}
