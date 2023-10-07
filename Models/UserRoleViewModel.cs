using System.Collections.Generic;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity;

public class UserRoleViewModel
{
    public string UserId { get; set; }
    public string RoleName { get; set; }
    public IEnumerable<ApplicationUser> Users { get; set; }
    public IEnumerable<IdentityRole> Roles { get; set; }
}
