using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;

namespace WSTickets.Api.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate();

        SeedCompanies(context);
        SeedRoles(context);
        SeedUsers(context);
        SeedTickets(context);

    }

    private static void SeedCompanies(AppDbContext context)
    {
        if (!context.Companies.Any())
        {
            context.Companies.AddRange(
                new Company { Name = "Wikibase Solutions" },
                new Company { Name = "DemoCorp" }
            );
            context.SaveChanges();
        }
    }

    private static void SeedRoles(AppDbContext context)
    {
        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Support" },
                new Role { Name = "Customer" }
            );
            context.SaveChanges();
        }
    }

    private static void SeedUsers(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var company = context.Companies.First();
            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var supportRole = context.Roles.First(r => r.Name == "Support");

            context.Users.AddRange(
                new User
                {
                    Username = "admin",
                    PasswordHash = "$2a$12$GJSiD6ZTA5MRrZQl5wGNTu3IKbeq9n1ANDqtH6fU2qCLMaqRh4lOO",
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    CompanyId = company.Id,
                    RoleId = adminRole.Id
                },
                new User
                {
                    Username = "support",
                    PasswordHash = "$2a$12$0WBV.vXjZe4mdOG166NLweRDeBhq1SrqoVrdyfJUnlHDpg8vrwnJO",
                    FullName = "Support User",
                    Email = "support@example.com",
                    CompanyId = company.Id,
                    RoleId = supportRole.Id
                }
            );
            context.SaveChanges();
        }
    }

    private static void SeedTickets(AppDbContext context)
    {
        if (!context.Tickets.Any())
        {
            var company = context.Companies.First();
            var reporter = context.Users.First(u => u.Username == "admin");

            context.Tickets.AddRange(
                new Ticket
                {
                    Title = "Demo ticket 1",
                    Description = "Dit is een voorbeeld ticket.",
                    Priority = TicketPriority.Medium,
                    CurrentStatus = TicketStatus.Open,
                    CompanyId = company.Id,
                    ReporterId = reporter.Id
                },
                new Ticket
                {
                    Title = "Demo ticket 2",
                    Description = "Tweede voorbeeldticket.",
                    Priority = TicketPriority.High,
                    CurrentStatus = TicketStatus.InProgress,
                    CompanyId = company.Id,
                    ReporterId = reporter.Id
                }
            );
            context.SaveChanges();
        }
    }
}
