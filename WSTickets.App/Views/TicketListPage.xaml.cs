using WSTickets.App.ViewModels;

namespace WSTickets.App.Views;

public partial class TicketListPage : ContentPage
{
    public TicketListPage()
    {
        InitializeComponent();
        BindingContext = new TicketListViewModel();
    }
}
