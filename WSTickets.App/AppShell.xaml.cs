namespace WSTickets.App;

using System.Net.Mail;
using WSTickets.App.Views;
using WSTickets.App.Services;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(TicketListPage), typeof(TicketListPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        //Routing.RegisterRoute(nameof(AllTicketsPage), typeof(AllTicketsPage));
        //Routing.RegisterRoute(nameof(NewTicketPage), typeof(NewTicketPage));
        //Routing.RegisterRoute(nameof(AccountPage), typeof(AccountPage));
        //Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));
        //Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));

        //Routing.RegisterRoute(nameof(TicketDetailPage), typeof(TicketDetailPage));
        //Routing.RegisterRoute(nameof(AddMessagePage), typeof(AddMessagePage));
        //Routing.RegisterRoute(nameof(ChangeStatusPage), typeof(ChangeStatusPage));
        //Routing.RegisterRoute(nameof(AttachmentPage), typeof(AttachmentPage));
        //Routing.RegisterRoute(nameof(CameraCapturePage), typeof(CameraCapturePage));
        //Routing.RegisterRoute(nameof(EditAccountPage), typeof(EditAccountPage));
        //Routing.RegisterRoute(nameof(MfaSettingsPage), typeof(MfaSettingsPage));
        //Routing.RegisterRoute(nameof(UserManagementPage), typeof(UserManagementPage));
        //Routing.RegisterRoute(nameof(EditUserPage), typeof(EditUserPage));
        //Routing.RegisterRoute(nameof(AssignRolesPage), typeof(AssignRolesPage));
        //Routing.RegisterRoute(nameof(TicketOverviewPage), typeof(TicketOverviewPage));

    //< !--All Tickets(only for staff / managers) -->
    //< FlyoutItem Title = "All Tickets" Icon = "list_alt" >
    //    < ShellContent Route = "AllTicketsPage" ContentTemplate = "{DataTemplate views:AllTicketsPage}" />
    //</ FlyoutItem >

    //< !--New Ticket-- >
    //< FlyoutItem Title = "New Ticket" Icon = "add_circle" >
    //    < ShellContent Route = "NewTicketPage" ContentTemplate = "{DataTemplate views:NewTicketPage}" />
    //</ FlyoutItem >

    //< !--My Account-- >
    //< FlyoutItem Title = "My Account" Icon = "account_circle" >
    //    < ShellContent Route = "AccountPage" ContentTemplate = "{DataTemplate views:AccountPage}" />
    //</ FlyoutItem >

    //< !--Admin(only for admin / manager) -->
    //< FlyoutItem Title = "Admin" Icon = "admin_panel_settings" >
    //    < ShellContent Route = "AdminDashboardPage" ContentTemplate = "{DataTemplate views:AdminDashboardPage}" />
    //</ FlyoutItem >

    //< !--Settings-- >
    //< FlyoutItem Title = "Settings" Icon = "settings" >
    //    < ShellContent Route = "SettingsPage" ContentTemplate = "{DataTemplate views:SettingsPage}" />
    //</ FlyoutItem >
    }
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await AuthService.Instance.LogoutAsync("You have been logged out.");
        App.NavigateToLoginPage();
    }

}
