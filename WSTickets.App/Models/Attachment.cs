using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Models;

public class Attachment
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public DateTime UploadedAt { get; set; }
    public int UploadedById { get; set; }
}
