namespace WSTickets.App;

using System.Net.Mail;
using WSTickets.App.Views;
using WSTickets.App.Services;

public partial class AppShell : Shell
{
        public AppShell()
        {
            InitializeComponent();

            // Routes
            Routing.RegisterRoute(nameof(TicketListPage), typeof(TicketListPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(AllTicketsPage), typeof(AllTicketsPage));
            Routing.RegisterRoute(nameof(NewTicketPage), typeof(NewTicketPage));
            Routing.RegisterRoute(nameof(TicketDetailPage), typeof(TicketDetailPage));
            Routing.RegisterRoute(nameof(ManageAccountsPage), typeof(ManageAccountsPage));
            Routing.RegisterRoute(nameof(NewAccountPage), typeof(NewAccountPage));


        // My Tickets
        Items.Add(new FlyoutItem
            {
                Title = "My Tickets",
                Icon = "inbox",
                Items =
            {
                new ShellContent
                {
                    Title = "My Tickets",
                    Route = nameof(TicketListPage),
                    ContentTemplate = new DataTemplate(typeof(TicketListPage))
                }
            }
            });

            // New Ticket
            Items.Add(new FlyoutItem
            {
                Title = "New Ticket",
                Icon = "add_circle",
                Items =
            {
                new ShellContent
                {
                    Title = "New Ticket",
                    Route = nameof(NewTicketPage),
                    ContentTemplate = new DataTemplate(typeof(NewTicketPage))
                }
            }
            });

            // Logout
            Items.Add(new MenuItem
            {
                Text = "Logout",
                IconImageSource = "logout",
                IsDestructive = true,
                Command = new Command(async () =>
                {
                    await AuthService.Instance.LogoutAsync("You have been logged out.");
                    App.NavigateToLoginPage();
                })
            });
        }

    public void AddRoleBasedFlyoutItems()
    {
        var allTicketsItem = Items.FirstOrDefault(i => i.Title == "All Tickets");
        if (allTicketsItem != null)
            Items.Remove(allTicketsItem);

        var role = AuthService.Instance.CurrentUserRole;

        if (role is "Manager" or "Admin")
        {
            Items.Insert(0, new FlyoutItem
            {
                Title = "All Tickets",
                Items =
            {
                new ShellContent
                {
                    Title = "All Tickets",
                    Route = nameof(AllTicketsPage),
                    ContentTemplate = new DataTemplate(typeof(AllTicketsPage))
                }
            }
            });

            Items.Insert(3, new FlyoutItem
            {
                Title = "Manage Accounts",
                Items =
            {
                new ShellContent
                {
                    Title = "Manage Accounts",
                    Route = nameof(ManageAccountsPage),
                    ContentTemplate = new DataTemplate(typeof(ManageAccountsPage))
                }
            }
            });

        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await AuthService.Instance.LogoutAsync("You have been logged out.");
        App.NavigateToLoginPage();
    }

}
