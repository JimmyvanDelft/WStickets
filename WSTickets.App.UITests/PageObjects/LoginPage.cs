namespace WSTickets.App.UITests.PageObjects;

public class LoginPage : BasePage
{
    private By UsernameField => By.XPath("(//android.widget.FrameLayout[@resource-id=\"com.companyname.wstickets.app:id/nav_host\"])[2]/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By PasswordField => By.XPath("(//android.widget.FrameLayout[@resource-id=\"com.companyname.wstickets.app:id/nav_host\"])[2]/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.FrameLayout/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By LoginButton => By.XPath("//android.widget.Button[@text=\"Log In\"]");

    public LoginPage(AndroidDriver driver) : base(driver) { }

    public void Login(string username, string password)
    {
        EnterText(UsernameField, username);
        EnterText(PasswordField, password);
        Tap(LoginButton);
    }

    public bool IsLoginButtonVisible() => IsDisplayed(LoginButton);
}
