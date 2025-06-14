using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Controllers;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;
using Xunit;

namespace WSTickets.Api.Tests.Controllers;

public class CompaniesControllerTests
{
    private AppDbContext _context;
    private CompaniesController _controller;

    public CompaniesControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("CompaniesTestDb_" + System.Guid.NewGuid())
            .Options;

        _context = new AppDbContext(options);

        _context.Companies.AddRange(
            new Company { Id = 1, Name = "Company One" },
            new Company { Id = 2, Name = "Company Two" }
        );

        _context.SaveChanges();

        _controller = new CompaniesController(_context);
        SetAdminUserContext();
    }

    private void SetAdminUserContext()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = principal }
        };
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnAllCompanies()
    {
        var result = await _controller.GetCompanies();

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var list = Assert.IsType<List<CompanyDto>>(ok.Value);
        Assert.Equal(2, list.Count);
    }

    [Fact]
    public async Task GetCompany_ShouldReturnCompany_WhenExists()
    {
        var result = await _controller.GetCompany(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var dto = Assert.IsType<CompanyDto>(ok.Value);
        Assert.Equal("Company One", dto.Name);
    }

    [Fact]
    public async Task GetCompany_ShouldReturnNotFound_WhenNotExists()
    {
        var result = await _controller.GetCompany(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCompany_ShouldAddNewCompany()
    {
        var dto = new CompanyCreateDto { Name = "New Co" };

        var result = await _controller.CreateCompany(dto);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CompanyDto>(created.Value);
        Assert.Equal("New Co", response.Name);

        var dbCompany = await _context.Companies.FindAsync(response.Id);
        Assert.NotNull(dbCompany);
    }

    [Fact]
    public async Task UpdateCompany_ShouldChangeName()
    {
        var update = new CompanyUpdateDto { Name = "Updated Co" };

        var result = await _controller.UpdateCompany(1, update);

        Assert.IsType<NoContentResult>(result);
        var company = await _context.Companies.FindAsync(1);
        Assert.Equal("Updated Co", company.Name);
    }

    [Fact]
    public async Task DeleteCompany_ShouldRemoveCompany()
    {
        var result = await _controller.DeleteCompany(2);

        Assert.IsType<NoContentResult>(result);
        var company = await _context.Companies.FindAsync(2);
        Assert.Null(company);
    }

    [Fact]
    public async Task DeleteCompany_ShouldReturnNotFound_WhenMissing()
    {
        var result = await _controller.DeleteCompany(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
