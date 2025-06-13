namespace WSTickets.App;

using Microsoft.Maui.Controls;
using WSTickets.App.Services;
using WSTickets.App.Views;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Temporary MainPage to show the login page
        MainPage = new NavigationPage(new LoadingPage());
        InitializeMainPageAsync();
    }

    private async void InitializeMainPageAsync()
    {
        var isLoggedIn = await AuthService.Instance.IsLoggedInAsync();

        if (isLoggedIn)
        {
            var shell = new AppShell();
            shell.AddRoleBasedFlyoutItems();
            MainPage = shell;
        }
        else
        {
            MainPage = new NavigationPage(new LoginPage());
        }
    }

    public static void NavigateToShell()
    {
        var shell = new AppShell();
        shell.AddRoleBasedFlyoutItems();
        Current.MainPage = shell;
    }

    public static void NavigateToLoginPage()
    {
        Current.MainPage = new NavigationPage(new LoginPage());
    }
}


