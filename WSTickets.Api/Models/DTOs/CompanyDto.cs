namespace WSTickets.Api.Models.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class CompanyCreateDto
{
    public string Name { get; set; }
}

public class CompanyUpdateDto
{
    public string Name { get; set; }
}
