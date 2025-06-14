using WSTickets.App.ViewModels;
using Xunit;

namespace WSTickets.App.Tests.ViewModels;

public class NewTicketViewModelTests
{
    [Fact]
    public void CreateCommand_ShouldBeDisabled_WhenTitleIsEmpty()
    {
        var vm = new NewTicketViewModel();
        vm.Title = "";

        var canExecute = vm.CreateTicketCommand.CanExecute(null);

        Assert.False(canExecute);
    }

    [Fact]
    public void CreateCommand_ShouldBeEnabled_WhenTitleIsPresent()
    {
        var vm = new NewTicketViewModel();
        vm.Title = "Test ticket";

        var canExecute = vm.CreateTicketCommand.CanExecute(null);

        Assert.True(canExecute);
    }

    [Fact]
    public void DefaultPriority_ShouldBeSet()
    {
        var vm = new NewTicketViewModel();

        Assert.NotNull(vm.SelectedPriority);
    }
}
