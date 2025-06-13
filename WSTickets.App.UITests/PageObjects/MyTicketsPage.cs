using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace WSTickets.App.UITests.PageObjects;

public class MyTicketsPage : BasePage
{
    public MyTicketsPage(AndroidDriver driver) : base(driver) { }

    // Filters
    private By SearchBox => By.XPath("(//android.widget.EditText)[1]");
    private By SearchLabel => By.XPath("//android.widget.TextView[@text='Search tickets']");
    private By ShowOnlyActive => By.XPath("//android.widget.TextView[@text='Show only active tickets']");
    private By FilterByStatus => By.XPath("//android.widget.TextView[@text='Filter by Status']");
    private By FilterByPriority => By.XPath("//android.widget.TextView[@text='Filter by Priority']");
    private By SortBy => By.XPath("//android.widget.TextView[@text='Sort by']");
    private By ClearFilters => By.XPath("//android.widget.Button[@text='Clear Filters']");

    // Tickets
    private By TicketWebsiteTraag => By.XPath("//android.widget.TextView[@text='Website laadt traag']");
    private By NoResultsMessage => By.XPath("//android.widget.TextView[contains(@text,'No tickets found')]");

    // Ticket detail
    private By TicketHeader => By.XPath("//android.widget.TextView[@text='Ticket #1']");
    private By PriorityLabel => By.XPath("//android.widget.TextView[@text='Priority:']");
    private By StatusLabel => By.XPath("//android.widget.TextView[@text='Status:']");
    private By AttachmentsLabel => By.XPath("//android.widget.TextView[@text='Attachments']");
    private By MessagesLabel => By.XPath("//android.widget.TextView[@text='Messages']");
    private By StatusHistoryLabel => By.XPath("//android.widget.TextView[@text='Status History']");
    private By MessageInput => By.XPath("//android.widget.EditText[@text='Type your message...']");
    private By SendButton => By.XPath("//android.widget.Button[@text='Send']");
    private By AddTicketButton => By.XPath("//android.widget.Button[@text='+']");

    // Messages
    private By Message1 => By.XPath("//android.widget.TextView[@text='Probleem sinds gisteren opgemerkt.']");
    private By Message2 => By.XPath("//android.widget.TextView[@text='Technisch onderzoek gestart.']");
    private By NewMessage(string text) => By.XPath($"//android.widget.TextView[@text='{text}']");

    // Actions
    public void Search(string text)
    {
        EnterText(SearchBox, text);
    }

    public void ClearSearch()
    {
        var el = FindElement(SearchBox);
        el.Clear();
    }

    public void OpenTicket() => Tap(TicketWebsiteTraag);

    public void SendMessage(string message)
    {
        EnterText(MessageInput, message);
        Tap(SendButton);
    }

    // Verifieer methoden
    public bool AreFilterElementsVisible() =>
        IsDisplayed(SearchLabel) &&
        IsDisplayed(ShowOnlyActive) &&
        IsDisplayed(FilterByStatus) &&
        IsDisplayed(FilterByPriority) &&
        IsDisplayed(SortBy) &&
        IsDisplayed(ClearFilters);

    public bool IsTicketVisible() => IsDisplayed(TicketWebsiteTraag);
    public bool IsNoResultVisible() => IsDisplayed(NoResultsMessage);
    public bool IsTicketDetailsVisible() =>
        IsDisplayed(TicketHeader) &&
        IsDisplayed(PriorityLabel) &&
        IsDisplayed(StatusLabel) &&
        IsDisplayed(AttachmentsLabel) &&
        IsDisplayed(MessagesLabel);

    public bool IsMessageVisible(string text) => IsDisplayed(NewMessage(text));

}
