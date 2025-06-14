using WSTickets.App.ViewModels;
using Xunit;

namespace WSTickets.App.Tests.ViewModels;

public class NewAccountViewModelTests
{
    [Fact]
    public void AvailableRoles_ShouldContainAllRoles()
    {
        var vm = new NewAccountViewModel();

        Assert.Contains(vm.AvailableRoles, r => r.Name == "Admin");
        Assert.Contains(vm.AvailableRoles, r => r.Name == "Manager");
        Assert.Contains(vm.AvailableRoles, r => r.Name == "Customer");
        Assert.Contains(vm.AvailableRoles, r => r.Name == "Support");
    }

    [Fact]
    public void Default_CompanySuggestions_ShouldBeEmpty()
    {
        var vm = new NewAccountViewModel();

        Assert.Empty(vm.CompanySuggestions);
    }
}
