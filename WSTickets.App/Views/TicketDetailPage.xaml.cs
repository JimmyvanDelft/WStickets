namespace WSTickets.App.Views;
using WSTickets.App.ViewModels;

[QueryProperty(nameof(TicketId), "id")]
public partial class TicketDetailPage : ContentPage
{
    private readonly TicketDetailViewModel _viewModel;

    public TicketDetailPage(TicketDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    private int _ticketId;
    public int TicketId
    {
        get => _ticketId;
        set
        {
            _ticketId = value;
            _ = _viewModel.LoadTicketAsync(_ticketId);
        }
    }
}
