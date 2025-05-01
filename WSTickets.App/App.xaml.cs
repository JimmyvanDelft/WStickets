namespace WSTickets.App;
using WSTickets.App.Services;
using WSTickets.App.Views;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        // Defer Init until UI is ready
        Application.Current.Dispatcher.Dispatch(async () => await InitAsync());
    }

    private async Task InitAsync()
    {
        var isLoggedIn = await AuthService.Instance.IsLoggedInAsync();

        if (isLoggedIn)
            await Shell.Current.GoToAsync("//TicketListPage");
        else
            await Shell.Current.GoToAsync("//LoginPage");
    }
}

