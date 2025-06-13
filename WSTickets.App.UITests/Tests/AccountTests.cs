using NUnit.Framework;
using WSTickets.App.UITests.Drivers;
using WSTickets.App.UITests.PageObjects;
using System;

namespace WSTickets.App.UITests.Tests;

[TestFixture]
public class AccountTests
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
    public void CanSeeAccountLabels()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("Manage Accounts");

        var page = new AccountPage(_driverManager.Driver);

        Assert.That(page.IsLabelVisible("Merel de Rooij"));
        Assert.That(page.IsLabelVisible("@MerelDeRooij"));
        Assert.That(page.IsLabelVisible("Wikibase Solutions"));
        Assert.That(page.IsLabelVisible("merel@wikibase.com"));
    }

    [Test]
    public void CanOpenCreateAccountPageAndSeeFields()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("Manage Accounts");

        var page = new AccountPage(_driverManager.Driver);
        page.TapAddAccountButton();

        Assert.That(page.IsCreateAccountTitleVisible(), "Create account title not visible");
        Assert.That(page.IsFullNameFieldVisible(), "Full name field not visible");
        Assert.That(page.IsUsernameFieldVisible(), "Username field not visible");
        Assert.That(page.IsPasswordFieldVisible(), "Password field not visible");
        Assert.That(page.IsEmailFieldVisible(), "Email field not visible");
        Assert.That(page.IsCompanyFieldVisible(), "Company field not visible");
        Assert.That(page.IsRoleDropdownVisible(), "Role dropdown not visible");
    }

    [Test]
    public void ShowsValidationWhenFieldsAreEmpty()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("Manage Accounts");

        var page = new AccountPage(_driverManager.Driver);
        page.TapAddAccountButton();
        page.TapCreateAccountButton();

        Assert.That(page.IsAlertDialogVisible(), Is.True);
    }

    [Test]
    public void CanCreateNewAccount()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        var flyout = new FlyoutMenuPage(_driverManager.Driver);
        flyout.OpenFlyout();
        flyout.TapMenuItem("Manage Accounts");

        var page = new AccountPage(_driverManager.Driver);
        page.TapAddAccountButton();

        var timestamp = DateTime.Now.ToString("HHmmss");
        string username = $"testuser{timestamp}";
        string email = $"{username}@mail.com";
        string company = "Bakkerij Pim Filius";

        page.FillAccountForm("Test Gebruiker", username, "pass123", email, company, "Customer");

        page.TapCreateAccountButton();

        Assert.That(page.IsLabelVisible($"@{username}"));
        Assert.That(page.IsLabelVisible(email));
        Assert.That(page.IsLabelVisible(company));
    }
}
