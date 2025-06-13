using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Support.UI;

namespace WSTickets.App.UITests.PageObjects;

public abstract class BasePage
{
    protected readonly AndroidDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(AndroidDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(TestConfiguration.Appium.ExplicitWaitSeconds));
    }

    protected IWebElement Find(By locator) => Wait.Until(d => d.FindElement(locator));
    protected IReadOnlyCollection<IWebElement> FindAll(By locator) => Driver.FindElements(locator);
    public void Tap(By locator) => Find(locator).Click();
    public void EnterText(By locator, string text)
    {
        var el = Find(locator);
        el.Clear();
        el.SendKeys(text);
    }
    public string GetText(By locator) => Find(locator).Text;
    public bool IsDisplayed(By locator)
    {
        try { return Driver.FindElement(locator).Displayed; }
        catch (NoSuchElementException) { return false; }
    }

    public IWebElement FindElement(By locator)
    {
        return Wait.Until(d => d.FindElement(locator));
    }

    public void WaitForElement(By locator, int timeoutInSeconds = 30)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutInSeconds));
        wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(locator);
                return element.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        });
    }


    public void ScrollTo(By locator)
    {
        const int maxScrolls = 5;
        for (int i = 0; i < maxScrolls; i++)
        {
            if (IsDisplayed(locator)) return;

            var js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("mobile: scrollGesture", new Dictionary<string, object>
            {
                { "left", 100 },
                { "top", 100 },
                { "width", 800 },
                { "height", 1000 },
                { "direction", "down" },
                { "percent", 0.8 }
            });

            Thread.Sleep(500);
        }

        throw new Exception("Element not found after scrolling.");
    }
}
