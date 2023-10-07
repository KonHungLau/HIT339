
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;


public class ManageRolesViewModel
{
    public List<ApplicationUser> Users { get; set; }
    public List<IdentityRole> Roles { get; set; }
}
