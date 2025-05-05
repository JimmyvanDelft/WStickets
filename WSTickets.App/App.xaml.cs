namespace WSTickets.App;
using WSTickets.App.Services;
using WSTickets.App.Views;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Temporary MainPage to show the login page
        MainPage = new ContentPage();
        InitializeMainPageAsync();
    }

    private async void InitializeMainPageAsync()
    {
        var isLoggedIn = await AuthService.Instance.IsLoggedInAsync();

        if (isLoggedIn)
        {
            MainPage = new AppShell();
        }
        else
        {
            MainPage = new NavigationPage(new LoginPage());
        }
    }

    public static void NavigateToShell()
    {
        Current.MainPage = new AppShell();
    }

    public static void NavigateToLoginPage()
    {
        Current.MainPage = new NavigationPage(new LoginPage());
    }
}


