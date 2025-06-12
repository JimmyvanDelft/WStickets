using NUnit.Framework;
using WSTickets.App.UITests.Drivers;

namespace WSTickets.App.UITests.Tests;

[TestFixture]
public class UnitTest1
{
    private AppiumDriverManager? _driverManager;

    [SetUp]
    public async Task Setup()
    {
        _driverManager = new AppiumDriverManager();
        await _driverManager.InitializeAsync();
    }

    [Test]
    public void AppLaunchesSuccessfully()
    {
        Assert.That(_driverManager, Is.Not.Null);
        var activity = _driverManager!.Driver.CurrentActivity;
        TestContext.WriteLine($"Current activity: {activity}");
        Assert.That(!string.IsNullOrWhiteSpace(activity));
    }

    [TearDown]
    public void TearDown()
    {
        _driverManager?.Quit();
    }
}
