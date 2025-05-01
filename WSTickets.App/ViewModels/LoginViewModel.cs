using System.Windows.Input;
using WSTickets.App.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WSTickets.App.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private string _username;
    private string _password;
    private string _errorMessage;
    private bool _hasError;

    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public bool HasError
    {
        get => _hasError;
        set { _hasError = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel()
    {
        LoginCommand = new Command(async () => await Login());
    }

    private async Task Login()
    {
        var success = await AuthService.Instance.LoginAsync(Username, Password);

        if (success)
        {
            // Ga naar hoofdpagina, bijvoorbeeld TicketListPage
            await Shell.Current.GoToAsync("//TicketListPage");
        }
        else
        {
            ErrorMessage = "Login mislukt. Controleer je gegevens.";
            HasError = true;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
