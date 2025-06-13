using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;

namespace WSTickets.App.UITests.PageObjects;

public class AccountPage : BasePage
{
    public AccountPage(AndroidDriver driver) : base(driver) { }

    private By AddAccountButton => By.XPath("//android.widget.Button[@bounds='[870,2127][1028,2285]']");
    private By CreateAccountTitle => By.XPath("//android.widget.TextView[@text=\"Create New Account\"]");
    private By FullNameField => By.XPath("//androidx.drawerlayout.widget.DrawerLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By UsernameField => By.XPath("//androidx.drawerlayout.widget.DrawerLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[2]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By PasswordField => By.XPath("//androidx.drawerlayout.widget.DrawerLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[3]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By EmailField => By.XPath("//androidx.drawerlayout.widget.DrawerLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[4]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.EditText");
    private By CompanyField => By.XPath("//android.widget.AutoCompleteTextView");
    private By RoleDropdown => By.XPath("//androidx.drawerlayout.widget.DrawerLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.widget.ScrollView/android.view.ViewGroup/android.view.ViewGroup/android.view.ViewGroup[6]/android.view.ViewGroup/android.view.ViewGroup[1]/android.view.ViewGroup/android.view.ViewGroup/android.widget.Button");
    private By RoleOption(string role) => By.XPath($"//android.widget.TextView[@resource-id='android:id/title' and @text='{role}']");
    private By CreateButton => By.XPath("//android.widget.Button[@text='Create Account']");
    private By AlertTitle => By.Id("com.companyname.wstickets.app:id/alertTitle");
    private By AlertMessage => By.Id("android:id/message");

    public void TapAddAccountButton() => Tap(AddAccountButton);

    public void TapCreateAccountButton() => Tap(CreateButton);

    public bool IsCreateAccountTitleVisible() => IsDisplayed(CreateAccountTitle);

    public bool IsFullNameFieldVisible() => IsDisplayed(FullNameField);
    public bool IsUsernameFieldVisible() => IsDisplayed(UsernameField);
    public bool IsPasswordFieldVisible() => IsDisplayed(PasswordField);
    public bool IsEmailFieldVisible() => IsDisplayed(EmailField);
    public bool IsCompanyFieldVisible() => IsDisplayed(CompanyField);
    public bool IsRoleDropdownVisible() => IsDisplayed(RoleDropdown);

    public void FillAccountForm(string name, string username, string password, string email, string company, string role)
    {
        EnterText(FullNameField, name);
        EnterText(UsernameField, username);
        EnterText(PasswordField, password);
        EnterText(EmailField, email);
        EnterText(CompanyField, company);

        Tap(RoleDropdown);
        Tap(RoleOption(role));
    }

    public bool IsAlertDialogVisible() =>
        IsDisplayed(AlertTitle) && IsDisplayed(AlertMessage);

    public bool IsLabelVisible(string labelText) =>
        IsDisplayed(By.XPath($"//android.widget.TextView[@text='{labelText}']"));
}
