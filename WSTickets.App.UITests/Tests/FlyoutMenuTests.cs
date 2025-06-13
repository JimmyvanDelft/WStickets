using NUnit.Framework;
using OpenQA.Selenium;
using WSTickets.App.UITests.Drivers;
using WSTickets.App.UITests.PageObjects;

namespace WSTickets.App.UITests.Tests;

[TestFixture]
public class FlyoutMenuTests
{
    private AppiumDriverManager? _driverManager;

    [SetUp]
    public async Task Setup()
    {
        _driverManager = new AppiumDriverManager();
        await _driverManager.InitializeAsync();
        LoginAsMerel();
    }

    [TearDown]
    public void TearDown()
    {
        _driverManager?.Quit();
    }

    private void LoginAsMerel()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");
    }

    [Test]
    public void FlyoutMenuItemsAreVisible()
    {
        var flyout = new FlyoutMenuPage(_driverManager!.Driver);
        flyout.OpenFlyout();

        Assert.Multiple(() =>
        {
            Assert.That(flyout.IsMenuItemVisible("All Tickets"), "All Tickets not visible");
            Assert.That(flyout.IsMenuItemVisible("My Tickets"), "My Tickets not visible");
            Assert.That(flyout.IsMenuItemVisible("New Ticket"), "New Ticket not visible");
            Assert.That(flyout.IsMenuItemVisible("Manage Accounts"), "Manage Accounts not visible");
            Assert.That(flyout.IsMenuItemVisible("Logout"), "Logout not visible");
        });
    }

    [Test]
    public void FlyoutMenuItemsNavigateToCorrectPages()
    {
        var flyout = new FlyoutMenuPage(_driverManager!.Driver);

        var menuItems = new[] { "All Tickets", "My Tickets", "New Ticket", "Manage Accounts" };

        foreach (var item in menuItems)
        {
            flyout.OpenFlyout();
            flyout.TapMenuItem(item);
            Assert.That(flyout.IsMenuItemVisible(item), $"Did not navigate to page: {item}");
        }
    }

    [Test]
    public void LogoutNavigatesToLoginPage()
    {
        var flyout = new FlyoutMenuPage(_driverManager!.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("Logout");

        var loginPage = new LoginPage(_driverManager.Driver);
        Assert.That(loginPage.IsLoginButtonVisible(), "Login button not visible after logout");
    }
}
