using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WSTickets.App.Models;
using WSTickets.App.Services;

namespace WSTickets.App.ViewModels;

public partial class NewAccountViewModel : ObservableObject
{
    [ObservableProperty] private string fullName;
    [ObservableProperty] private string username;
    [ObservableProperty] private string email;
    [ObservableProperty] private string companyInput;
    [ObservableProperty] private string selectedRole;
    [ObservableProperty] private ObservableCollection<CompanyDto> companySuggestions = new();

    public ObservableCollection<string> AvailableRoles { get; } = new() { "Customer", "Developer", "Manager", "Admin" };

    public IAsyncRelayCommand CreateAccountCommand { get; }

    public NewAccountViewModel()
    {
        CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);
        _ = LoadCompaniesAsync();
    }

    private async Task LoadCompaniesAsync()
    {
        var companies = await CompanyService.Instance.GetCompaniesAsync();
        CompanySuggestions = new ObservableCollection<CompanyDto>(companies);
    }

    private async Task CreateAccountAsync()
    {
        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(CompanyInput) ||
            string.IsNullOrWhiteSpace(SelectedRole))
        {
            await Application.Current.MainPage.DisplayAlert("Missing info", "Please fill in all fields.", "OK");
            return;
        }

        var matchedCompany = CompanySuggestions.FirstOrDefault(c =>
            string.Equals(c.Name, CompanyInput.Trim(), StringComparison.OrdinalIgnoreCase));

        CompanyDto finalCompany;
        if (matchedCompany is not null)
        {
            finalCompany = matchedCompany;
        }
        else
        {
            finalCompany = await CompanyService.Instance.CreateCompanyAsync(CompanyInput.Trim());
            if (finalCompany == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not create company.", "OK");
                return;
            }

            CompanySuggestions.Add(finalCompany);
        }

        var result = await UserService.Instance.CreateUserAsync(new UserCreateDto
        {
            FullName = FullName,
            Username = Username,
            Email = Email,
            RoleName = SelectedRole,
            CompanyId = finalCompany.Id
        });

        if (result)
            await Application.Current.MainPage.DisplayAlert("Success", "User created.", "OK");
        else
            await Application.Current.MainPage.DisplayAlert("Error", "User creation failed.", "OK");
    }
}

