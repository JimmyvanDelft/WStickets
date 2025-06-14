using WSTickets.App.Models;
using WSTickets.App.ViewModels;
using Xunit;
using WSTickets.App.Services;

namespace WSTickets.App.Tests.ViewModels;

public class TicketDetailViewModelTests
{
    [Fact]
    public void IsSendEnabled_ShouldBeTrue_WhenMessageIsNotEmpty()
    {
        var vm = new TicketDetailViewModel();
        vm.NewMessageContent = "Hello";

        Assert.True(vm.IsSendEnabled);
    }

    [Fact]
    public void IsSendEnabled_ShouldBeFalse_WhenMessageIsEmpty()
    {
        var vm = new TicketDetailViewModel();
        vm.NewMessageContent = "";

        Assert.False(vm.IsSendEnabled);
    }

    [Fact]
    public void CanEditTicket_ShouldBeTrue_ForAdminRole()
    {
        AuthService.Instance.CurrentUserRole = "Admin";
        var vm = new TicketDetailViewModel();

        Assert.True(vm.CanEditTicket);
    }

    [Fact]
    public void CanEditTicket_ShouldBeFalse_ForCustomerRole()
    {
        AuthService.Instance.CurrentUserRole = "Customer";
        var vm = new TicketDetailViewModel();

        Assert.False(vm.CanEditTicket);
    }
}
