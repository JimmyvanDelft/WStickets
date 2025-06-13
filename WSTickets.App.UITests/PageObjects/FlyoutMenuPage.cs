using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace WSTickets.App.UITests.PageObjects;

public class FlyoutMenuPage : BasePage
{
    public FlyoutMenuPage(AndroidDriver driver) : base(driver) { }

    private By OpenFlyoutButton => By.XPath("//android.widget.ImageButton[@content-desc='Open navigation drawer']");
    private By AllTickets => By.XPath("//android.widget.TextView[@text='All Tickets']");
    private By MyTickets => By.XPath("(//android.widget.TextView[@text='My Tickets'])[2]");
    private By NewTicket => By.XPath("//android.widget.TextView[@text='New Ticket']");
    private By ManageAccounts => By.XPath("//android.widget.TextView[@text='Manage Accounts']");
    private By Logout => By.XPath("//android.widget.TextView[@text='Logout']");

    public void OpenFlyout() => Tap(OpenFlyoutButton);

    public bool IsMenuItemVisible(string label)
    {
        var xpath = $"//android.widget.TextView[@text='{label}']";
        return IsDisplayed(By.XPath(xpath));
    }

    public void TapMenuItem(string label)
    {
        Tap(By.XPath($"//android.widget.TextView[@text='{label}']"));
    }
}
