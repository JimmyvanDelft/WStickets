using NUnit.Framework;
using WSTickets.App.UITests.Drivers;
using WSTickets.App.UITests.PageObjects;
using System;

namespace WSTickets.App.UITests.Tests;

[TestFixture]
public class NewTicketTests
{
    private AppiumDriverManager? _driverManager;

    [SetUp]
    public async Task Setup()
    {
        _driverManager = new AppiumDriverManager();
        await _driverManager.InitializeAsync();
    }

    [TearDown]
    public void TearDown()
    {
        _driverManager?.Quit();
    }

    [Test]
    public void CanNavigateToNewTicketPage()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("New Ticket");

        Assert.That(flyout.IsMenuItemVisible("New Ticket"), Is.True, "New Ticket pagina titel ontbreekt");
    }

    [Test]
    public void NewTicketPageShowsCorrectFields()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("New Ticket");

        var newTicketPage = new NewTicketPage(_driverManager.Driver);

        Assert.Multiple(() =>
        {
            Assert.That(newTicketPage.IsTitleFieldVisible(), Is.True, "Titelveld ontbreekt");
            Assert.That(newTicketPage.IsDescriptionFieldVisible(), Is.True, "Beschrijvingveld ontbreekt");
            Assert.That(newTicketPage.IsPriorityLabelVisible(), Is.True, "Priority label ontbreekt");
        });
    }

    [Test]
    public void CanCreateTicketAndSeeDetails()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("New Ticket");

        var newTicketPage = new NewTicketPage(_driverManager.Driver);

        string title = "TestTicket";
        string description = $"Autotest {DateTime.Now:yyyyMMdd_HHmmss}";

        newTicketPage.EnterTitle(title);
        newTicketPage.EnterDescription(description);
        newTicketPage.TapCreateTicket();

        var detailPage = new TicketDetailPage(_driverManager.Driver);
        detailPage.WaitForAttachmentLabel();

        Assert.Multiple(() =>
        {
            Assert.That(detailPage.IsAttachmentLabelVisible(), Is.True);
            Assert.That(detailPage.IsTicketTitleVisible(title), Is.True);
            Assert.That(detailPage.IsTicketDescriptionVisible(description), Is.True);
        });
    }
}
