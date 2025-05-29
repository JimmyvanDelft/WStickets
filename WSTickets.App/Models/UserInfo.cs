using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSTickets.App.Models;

public class UserInfoDto
{
    public int UserId { get; set; }
    public string Role { get; set; } = string.Empty;
}