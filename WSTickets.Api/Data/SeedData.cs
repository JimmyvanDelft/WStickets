using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Models.Entities;
using WSTickets.Api.Models.Enums;
using BCrypt.Net;

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
                new Company { Name = "Bakkerij Pim Filius" }
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
                new Role { Name = "Manager" },
                new Role { Name = "Customer" },
                new Role { Name = "Support" }
            );
            context.SaveChanges();
        }
    }

    private static void SeedUsers(AppDbContext context)
    {
        if (!context.Users.Any())
        {
            var wikibase = context.Companies.First(c => c.Name == "Wikibase Solutions");
            var bakkerijPim = context.Companies.First(c => c.Name == "Bakkerij Pim Filius");

            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var managerRole = context.Roles.First(r => r.Name == "Manager");
            var customerRole = context.Roles.First(r => r.Name == "Customer");

            context.Users.AddRange(
                new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    FullName = "Admin User",
                    Email = "admin@wikibase.com",
                    CompanyId = wikibase.Id,
                    RoleId = adminRole.Id
                },
                new User
                {
                    Username = "MerelDeRooij",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("merel123"),
                    FullName = "Merel de Rooij",
                    Email = "merel@wikibase.com",
                    CompanyId = wikibase.Id,
                    RoleId = managerRole.Id
                },
                new User
                {
                    Username = "PimFilius",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("pim123"),
                    FullName = "Pim Filius",
                    Email = "pim@bakkerijpim.nl",
                    CompanyId = bakkerijPim.Id,
                    RoleId = customerRole.Id
                }
            );
            context.SaveChanges();
        }
    }

    private static void SeedTickets(AppDbContext context)
    {
        if (!context.Tickets.Any())
        {
            var wikibase = context.Companies.First(c => c.Name == "Wikibase Solutions");
            var adminUser = context.Users.First(u => u.Username == "admin");
            var merelUser = context.Users.First(u => u.Username == "MerelDeRooij");
            var pimUser = context.Users.First(u => u.Username == "PimFilius");

            var ticket1 = new Ticket
            {
                Title = "Website laadt traag",
                Description = "De homepage van onze website laadt erg langzaam.",
                Priority = TicketPriority.High,
                CurrentStatus = TicketStatus.InProgress,
                CompanyId = wikibase.Id,
                ReporterId = merelUser.Id,
                AssigneeId = adminUser.Id,
                Messages = new List<Message>
            {
                new Message
                {
                    Content = "Probleem sinds gisteren opgemerkt.",
                    IsInternal = false,
                    Timestamp = DateTime.UtcNow.AddMinutes(-90),
                    AuthorId = merelUser.Id
                },
                new Message
                {
                    Content = "Technisch onderzoek gestart.",
                    IsInternal = true,
                    Timestamp = DateTime.UtcNow.AddMinutes(-60),
                    AuthorId = adminUser.Id
                },
                new Message
                {
                    Content = "Cache-instellingen aangepast, graag opnieuw testen.",
                    IsInternal = false,
                    Timestamp = DateTime.UtcNow.AddMinutes(-30),
                    AuthorId = adminUser.Id
                }
            }
            };

            var ticket2 = new Ticket
            {
                Title = "Factuur ontbreekt",
                Description = "Ik kan de factuur van maart niet terugvinden in het klantportaal.",
                Priority = TicketPriority.Medium,
                CurrentStatus = TicketStatus.Resolved,
                CompanyId = wikibase.Id,
                ReporterId = pimUser.Id,
                AssigneeId = adminUser.Id,
                Messages = new List<Message>
            {
                new Message
                {
                    Content = "Factuur 2025-03 ontbreekt.",
                    IsInternal = false,
                    Timestamp = DateTime.UtcNow.AddHours(-5),
                    AuthorId = pimUser.Id
                },
                new Message
                {
                    Content = "Factuur opnieuw gestuurd per e-mail.",
                    IsInternal = true,
                    Timestamp = DateTime.UtcNow.AddHours(-4),
                    AuthorId = adminUser.Id
                },
                new Message
                {
                    Content = "Dank, ik heb 'm ontvangen.",
                    IsInternal = false,
                    Timestamp = DateTime.UtcNow.AddHours(-3),
                    AuthorId = pimUser.Id
                }
            }
            };

            context.Tickets.AddRange(ticket1, ticket2);
            context.SaveChanges();

            context.StatusHistories.AddRange(
                // Ticket 1
                new StatusHistory
                {
                    TicketId = ticket1.Id,
                    Status = TicketStatus.Open,
                    Timestamp = DateTime.UtcNow.AddHours(-2),
                    ChangedById = merelUser.Id
                },
                new StatusHistory
                {
                    TicketId = ticket1.Id,
                    Status = TicketStatus.InProgress,
                    Timestamp = DateTime.UtcNow.AddHours(-1),
                    ChangedById = adminUser.Id
                },

                // Ticket 2
                new StatusHistory
                {
                    TicketId = ticket2.Id,
                    Status = TicketStatus.Open,
                    Timestamp = DateTime.UtcNow.AddHours(-5),
                    ChangedById = pimUser.Id
                },
                new StatusHistory
                {
                    TicketId = ticket2.Id,
                    Status = TicketStatus.InProgress,
                    Timestamp = DateTime.UtcNow.AddHours(-4),
                    ChangedById = adminUser.Id
                },
                new StatusHistory
                {
                    TicketId = ticket2.Id,
                    Status = TicketStatus.Resolved,
                    Timestamp = DateTime.UtcNow.AddHours(-3),
                    ChangedById = adminUser.Id
                }
            );

            context.SaveChanges();
        }
    }

}
