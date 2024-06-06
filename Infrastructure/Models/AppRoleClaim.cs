using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;
public class AppRoleClaim:IdentityRoleClaim<string>
{
    public string Description { get; set; }
    public string Group { get; set; }
}
