using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AnyoneForTennis.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using AnyoneForTennis.Data;

public class UserRoleManagementController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public UserRoleManagementController(UserManager<ApplicationUser> userManager,
                                         RoleManager<IdentityRole> roleManager,
                                         ApplicationDbContext context) // 添加 ApplicationDbContext context 参数
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context; // 使用传入的 context 参数初始化 _context
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult ManageRoles()
    {
        // Populate the ViewModel with Users and Roles information.
        var model = new ManageRolesViewModel
        {
            Users = _userManager.Users.ToList(),
            Roles = _roleManager.Roles.ToList()
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to locate User with ID {userId}");
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded)
        {
            // 检查是否已经为该用户创建了 Coach 实体
            var existingCoach = await _context.Coaches.FindAsync(userId);
            if (existingCoach == null && roleName == "Coach")
            {
                // 如果还没有创建 Coach 实体，则创建一个新的
                var newCoach = new Coach
                {
                    CoachId = userId,
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    Biography = user.Biography 
                };
                _context.Coaches.Add(newCoach);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = $"User {user.UserName} has been successfully assigned to role {roleName}.";
            return RedirectToAction("ManageRoles");
        }
        else
        {
            TempData["ErrorMessage"] = $"Error assigning role: {result.Errors.FirstOrDefault()?.Description}";
            return RedirectToAction("ManageRoles");
        }
    }



    [HttpPost]
    public async Task<IActionResult> RemoveRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            TempData["ErrorMessage"] = $"Unable to locate User with ID {userId}";
            return RedirectToAction("ManageRoles");
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            TempData["ErrorMessage"] = $"User is not in role '{roleName}'";
            return RedirectToAction("ManageRoles");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (!result.Succeeded)
        {
            // Handle Error
            TempData["ErrorMessage"] = "Error removing role";
            return RedirectToAction("ManageRoles");
        }

        TempData["SuccessMessage"] = $"Successfully removed {user.UserName} from role '{roleName}'";
        return RedirectToAction("ManageRoles");
    }
}


