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
        _ = LoadUsersAsync();
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

}
