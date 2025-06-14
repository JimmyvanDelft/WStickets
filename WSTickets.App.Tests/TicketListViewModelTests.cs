using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WSTickets.App.Models;
using WSTickets.App.ViewModels;
using Xunit;

namespace WSTickets.App.Tests.ViewModels;

public class TicketListViewModelTests
{
    [Fact]
    public void ApplyFilters_ShouldFilterByStatus()
    {
        var vm = new TicketListViewModel();
        var tickets = new List<Ticket>
        {
            new Ticket { Id = 1, Title = "Bug", CurrentStatus = TicketStatus.Open },
            new Ticket { Id = 2, Title = "Fix", CurrentStatus = TicketStatus.Closed },
        };

        vm.GetType().GetField("_allTickets", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(vm, tickets);
        vm.SelectedStatusString = "New";

        vm.ApplyFiltersCommand.Execute(null);

        Assert.Single(vm.Tickets);
        Assert.Equal("Bug", vm.Tickets[0].Title);
    }

    [Fact]
    public void ApplyFilters_ShouldFilterBySearchQuery()
    {
        var vm = new TicketListViewModel();
        var tickets = new List<Ticket>
        {
            new Ticket { Id = 1, Title = "Network issue", Description = "Cannot connect" },
            new Ticket { Id = 2, Title = "Login bug", Description = "Wrong password" }
        };

        vm.GetType().GetField("_allTickets", BindingFlags.NonPublic | BindingFlags.Instance)!
            .SetValue(vm, tickets);
        vm.SearchQuery = "login";

        vm.ApplyFiltersCommand.Execute(null);

        Assert.Single(vm.Tickets);
        Assert.Equal("Login bug", vm.Tickets[0].Title);
    }
}
