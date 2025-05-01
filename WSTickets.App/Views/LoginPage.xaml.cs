namespace WSTickets.App.Views;
using WSTickets.App.ViewModels;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }
}