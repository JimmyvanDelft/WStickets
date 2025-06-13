using NUnit.Framework;
using WSTickets.App.UITests.Drivers;
using WSTickets.App.UITests.PageObjects;

namespace WSTickets.App.UITests.Tests;

[TestFixture]
public class LoginTests
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
    public void CanLoginWithValidCredentials()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("MerelDeRooij", "merel123");

        Assert.That(loginPage.IsDisplayed(By.XPath("//android.widget.TextView[@text='My Tickets']")));
    }

    [Test]
    public void ShowsErrorWithInvalidCredentials()
    {
        var loginPage = new LoginPage(_driverManager!.Driver);
        loginPage.Login("wronguser", "wrongpass");

        var errorMessageLocator = By.XPath("//android.widget.TextView[@text='Incorrect username or password.']");
        Assert.That(loginPage.IsDisplayed(errorMessageLocator), "Expected error message was not displayed.");
    }
}
