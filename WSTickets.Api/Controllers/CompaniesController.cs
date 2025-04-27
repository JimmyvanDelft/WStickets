using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WSTickets.Api.Data;
using WSTickets.Api.Models.DTOs;
using WSTickets.Api.Models.Entities;

namespace WSTickets.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompaniesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/companies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
    {
        var companies = await _context.Companies
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return Ok(companies);
    }

    // GET: api/companies/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDto>> GetCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
            return NotFound();

        var companyDto = new CompanyDto
        {
            Id = company.Id,
            Name = company.Name
        };

        return Ok(companyDto);
    }

    // POST: api/companies
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyCreateDto companyDto)
    {
        var newCompany = new Company
        {
            Name = companyDto.Name
        };

        _context.Companies.Add(newCompany);
        await _context.SaveChangesAsync();

        var createdCompanyDto = new CompanyDto
        {
            Id = newCompany.Id,
            Name = newCompany.Name
        };

        return CreatedAtAction(nameof(GetCompany), new { id = newCompany.Id }, createdCompanyDto);
    }

    // PUT: api/companies/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCompany(int id, CompanyUpdateDto companyDto)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
            return NotFound();

        company.Name = companyDto.Name;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/companies/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCompany(int id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company == null)
            return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
