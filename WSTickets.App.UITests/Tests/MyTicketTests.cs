using NUnit.Framework;
using WSTickets.App.UITests.Drivers;
using WSTickets.App.UITests.PageObjects;

namespace WSTickets.App.UITests.Tests;

[TestFixture]
public class MyTicketsTests
{
    private AppiumDriverManager? _driverManager;

    [SetUp]
    public async Task Setup()
    {
        _driverManager = new AppiumDriverManager();
        await _driverManager.InitializeAsync();

        // Login
        var loginPage = new LoginPage(_driverManager.Driver);
        loginPage.Login("MerelDeRooij", "merel123");
    }

    [TearDown]
    public void TearDown()
    {
        _driverManager?.Quit();
    }

    [Test]
    public void FilterOptionsAreVisible()
    {
        var page = new MyTicketsPage(_driverManager!.Driver);
        Assert.That(page.AreFilterElementsVisible(), Is.True, "Not all filter elements visible.");
    }

    [Test]
    public void SearchReturnsCorrectTicket()
    {
        var page = new MyTicketsPage(_driverManager!.Driver);
        page.Search("Website laadt traag");

        Assert.That(page.IsTicketVisible(), Is.True, "Ticket not found");
    }

    [Test]
    public void SearchReturnsNoResults()
    {
        var page = new MyTicketsPage(_driverManager!.Driver);
        page.Search("stroopwafel");

        Assert.That(page.IsNoResultVisible(), Is.True, "Notickets message not shown");
        page.ClearSearch();
    }

    [Test]
    public void TicketDetailsAreVisible()
    {
        var page = new MyTicketsPage(_driverManager!.Driver);
        page.OpenTicket();

        Assert.That(page.IsTicketDetailsVisible(), Is.True, "Not all ticket details visible");
    }

    [Test]
    public void CanSendAndSeeNewMessage()
    {
        var page = new MyTicketsPage(_driverManager!.Driver);
        page.OpenTicket();

        Assert.That(page.IsMessageVisible("Probleem sinds gisteren opgemerkt."), Is.True);
        Assert.That(page.IsMessageVisible("Technisch onderzoek gestart."), Is.True);

        const string newMessage = "Probleem opgelost";
        page.SendMessage(newMessage);

        Assert.That(page.IsMessageVisible(newMessage), Is.True, "Messages not instantly visible");
    }

}
