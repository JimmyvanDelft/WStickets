using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Services;


namespace WSTickets.App.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string username;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string errorMessage;

    [ObservableProperty]
    private bool hasError;

    public LoginViewModel()
    {
        LoginCommand = new AsyncRelayCommand(Login);
    }

    public IAsyncRelayCommand LoginCommand { get; }

    private async Task Login()
    {
        var (success, errorMessage) = await AuthService.Instance.LoginAsync(Username, Password);

        System.Diagnostics.Debug.WriteLine($"[LoginViewModel] Login success: {success}, error: {errorMessage}");

        if (success)
        {
            HasError = false;
            App.NavigateToShell();
            await Shell.Current.GoToAsync("//TicketListPage");
        }
        else
        {
            ErrorMessage = errorMessage;
            HasError = true;
        }
    }
}
