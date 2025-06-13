using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace WSTickets.App.UITests.PageObjects;

public class NewTicketPage : BasePage
{
    private By TitleField => By.XPath("//androidx.viewpager.widget.ViewPager/androidx.recyclerview.widget.RecyclerView/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By DescriptionField => By.XPath("//androidx.viewpager.widget.ViewPager/androidx.recyclerview.widget.RecyclerView/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By PriorityLabel => By.XPath("//android.widget.TextView[@text='Priority']");
    private By CreateButton => By.XPath("//android.widget.Button[@text='Create Ticket']");

    public NewTicketPage(AndroidDriver driver) : base(driver) { }

    public void EnterTitle(string title) => EnterText(TitleField, title);

    public void EnterDescription(string desc) => EnterText(DescriptionField, desc);

    public bool IsPriorityLabelVisible() => IsDisplayed(PriorityLabel);

    public void TapCreateTicket()
    {
        WaitForElement(CreateButton);
        Tap(CreateButton);
    }

    public bool IsTitleFieldVisible() => IsDisplayed(TitleField);
    public bool IsDescriptionFieldVisible() => IsDisplayed(DescriptionField);

}
