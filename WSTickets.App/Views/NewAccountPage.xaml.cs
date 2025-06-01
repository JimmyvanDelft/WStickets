using WSTickets.App.ViewModels;

namespace WSTickets.App.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
        BindingContext = new NewAccountViewModel();
    }
}
