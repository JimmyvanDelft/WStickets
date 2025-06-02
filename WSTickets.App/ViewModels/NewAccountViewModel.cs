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
    [ObservableProperty] private string password;
    [ObservableProperty] private RoleOption selectedRole;
    [ObservableProperty] private ObservableCollection<string> companySuggestions = new();

    public IAsyncRelayCommand CreateAccountCommand { get; }

    public NewAccountViewModel()
    {
        CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);
        _ = LoadCompaniesAsync();
    }

    private async Task LoadCompaniesAsync()
    {
        var companies = await CompanyService.Instance.GetCompaniesAsync();
        CompanySuggestions = new ObservableCollection<string>(companies.Select(c => c.Name));
    }

    private async Task CreateAccountAsync()
    {

        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(CompanyInput) ||
            string.IsNullOrWhiteSpace(Password) ||
            SelectedRole == null)
        {
            await Application.Current.MainPage.DisplayAlert("Missing info", "Please fill in all fields.", "OK");
            return;
        }

        var matchedCompanyName = CompanySuggestions.FirstOrDefault(name =>
            string.Equals(name, CompanyInput.Trim(), StringComparison.OrdinalIgnoreCase));

        CompanyDto finalCompany;

        if (matchedCompanyName != null)
        {
            finalCompany = (await CompanyService.Instance.GetCompaniesAsync())
                .FirstOrDefault(c => c.Name.Equals(matchedCompanyName, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            finalCompany = await CompanyService.Instance.CreateCompanyAsync(CompanyInput.Trim());
        }

        if (finalCompany == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Could not create company.", "OK");
            return;
        }

        var dto = new UserCreateDto
        {
            FullName = FullName,
            Username = Username,
            Email = Email,
            Password = Password,
            CompanyId = finalCompany.Id,
            RoleId = SelectedRole.Id
        };

        var result = await UserService.Instance.CreateUserAsync(dto);

        if (result)
            await Shell.Current.GoToAsync("..");
        else
            await Application.Current.MainPage.DisplayAlert("Error", "User creation failed.", "OK");
    }


    public ObservableCollection<RoleOption> AvailableRoles { get; } = new()
{
    new RoleOption { Id = 1, Name = "Admin" },
    new RoleOption { Id = 2, Name = "Manager" },
    new RoleOption { Id = 3, Name = "Customer" },
    new RoleOption { Id = 4, Name = "Support" }
};
    public class RoleOption
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;

        public override bool Equals(object? obj)
        {
            return obj is RoleOption other && other.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}

