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

    [ObservableProperty] 
    private bool isBusy;

    public LoginViewModel()
    {
        LoginCommand = new AsyncRelayCommand(LoginAsync, () => !IsBusy);
    }

    public IAsyncRelayCommand LoginCommand { get; }

    private async Task LoginAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        LoginCommand.NotifyCanExecuteChanged();

        try
        {
            var (success, errorMessage) = await AuthService.Instance.LoginAsync(Username, Password);

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
        finally
        {
            IsBusy = false;
            LoginCommand.NotifyCanExecuteChanged();
        }
    }
}
