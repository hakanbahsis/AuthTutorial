using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;
public class AppRole:IdentityRole
{
    public string Description { get; set; }
}
