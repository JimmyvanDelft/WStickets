namespace WSTickets.Api.Models.DTOs;

public class AttachmentDto
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public DateTime UploadedAt { get; set; }
    public int UploadedById { get; set; }
    public string? UploadedByName { get; set; }
}

public class AttachmentCreateDto
{
    public string FilePath { get; set; }
    public string FileType { get; set; }
}
