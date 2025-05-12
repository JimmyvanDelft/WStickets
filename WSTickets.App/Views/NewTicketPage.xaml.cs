using WSTickets.App.ViewModels;

namespace WSTickets.App.Views;

public partial class NewTicketPage : ContentPage
{
	public NewTicketPage()
	{
		InitializeComponent();
        BindingContext = new NewTicketViewModel();
    }
}