using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace WSTickets.App.UITests.PageObjects;

public class TicketDetailPage : BasePage
{
    public TicketDetailPage(AndroidDriver driver) : base(driver) { }

    private By AttachmentsLabel => By.XPath("//android.widget.TextView[@text='Attachments']");

    public void WaitForAttachmentLabel() => WaitForElement(AttachmentsLabel);

    public bool IsAttachmentLabelVisible() => IsDisplayed(AttachmentsLabel);

    public bool IsTicketTitleVisible(string title)
    {
        return IsDisplayed(By.XPath($"//android.widget.TextView[@text='{title}']"));
    }

    public bool IsTicketDescriptionVisible(string description)
    {
        return IsDisplayed(By.XPath($"//android.widget.TextView[@text='{description}']"));
    }
}
