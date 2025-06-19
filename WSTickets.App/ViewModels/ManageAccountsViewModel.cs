using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels;

public partial class ManageAccountsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string errorMessage;

    public ObservableCollection<User> Users { get; } = new();

    public ManageAccountsViewModel()
    {
        LoadUsersCommand = new AsyncRelayCommand(LoadUsersAsync);
    }

    public IAsyncRelayCommand LoadUsersCommand { get; }

    private async Task LoadUsersAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;
        Users.Clear();

        try
        {
            var users = await UserService.Instance.GetUsersAsync();
            var sorted = users.OrderBy(u => u.FullName);

            foreach (var user in sorted)
                Users.Add(user);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load users: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task RefreshAsync()
    {
        await LoadUsersAsync();
    }

    [RelayCommand]
    private async Task OnUserLongPressed(User user)
    {
        if (user == null) return;

        string action = await Shell.Current.DisplayActionSheet(
            $"Acties voor {user.FullName}", "Annuleren", null, "Verwijderen");

        if (action == "Verwijderen")
            await DeleteUserAsync(user);
    }

    [RelayCommand]
    private async Task DeleteUser(User user)
    {
        if (user == null) return;

        await DeleteUserAsync(user);
    }


    private async Task DeleteUserAsync(User user)
    {
        bool confirm = await Shell.Current.DisplayAlert(
            "Bevestigen", $"Weet je zeker dat je {user.FullName} wilt verwijderen?", "Ja", "Nee");

        if (!confirm) return;

        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var success = await UserService.Instance.DeleteUserAsync(user.Id);
            if (success)
            {
                Users.Remove(user);
            }
            else
            {
                ErrorMessage = "Verwijderen is mislukt.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Fout bij verwijderen: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

}
