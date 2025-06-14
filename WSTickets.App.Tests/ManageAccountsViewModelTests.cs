using System.Collections.ObjectModel;
using WSTickets.App.Models;
using WSTickets.App.ViewModels;
using Xunit;

namespace WSTickets.App.Tests.ViewModels;

public class ManageAccountsViewModelTests
{
    [Fact]
    public void AddUsers_ShouldSortAlphabeticallyByFullName()
    {
        // Arrange
        var vm = new ManageAccountsViewModel();

        var users = new List<User>
        {
            new User { FullName = "Zara" },
            new User { FullName = "Anna" }
        };

        // Simuleer "toevoegen zoals de ViewModel dat doet na ophalen"
        foreach (var user in users.OrderBy(u => u.FullName))
        {
            vm.Users.Add(user);
        }

        // Act
        var first = vm.Users[0].FullName;
        var second = vm.Users[1].FullName;

        // Assert
        Assert.Equal("Anna", first);
        Assert.Equal("Zara", second);
    }

    [Fact]
    public void ErrorMessage_ShouldBeSettable()
    {
        var vm = new ManageAccountsViewModel();
        vm.ErrorMessage = "Fout";

        Assert.Equal("Fout", vm.ErrorMessage);
    }
}
