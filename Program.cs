using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AnyoneForTennis.Data;
using AnyoneForTennis.Models;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddRoles<IdentityRole>() // Add role support
.AddEntityFrameworkStores<ApplicationDbContext>();

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("MemberPolicy", policy => policy.RequireRole("Member"));
    options.AddPolicy("CoachPolicy", policy => policy.RequireRole("Coach"));
});

// Add MVC and JSON serialization settings
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

// Configure the application cookie policy
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Coach/Dashboard"; 
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    // ... other options
});

// Configure the general cookie policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    // ... other options
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        SeedData.Initialize(services); // Initialize roles and users

        ApplicationUser user = await userManager.FindByEmailAsync("admin@e.com");

        if (user != null)
        {
            var isUserInRole = await userManager.IsInRoleAsync(user, "Admin");
            if (!isUserInRole)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating roles or assigning roles to users.");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHttpsRedirection(); // Enable HTTPS in the development environment
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Enable HTTPS in production
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "{controller=UserRoleManagementController}/{action=ManageRoles}/{id?}");

    endpoints.MapControllerRoute(
        name: "adminDashboard",
        pattern: "Admin/{action=AdminDashboard}/{id?}",
        defaults: new { controller = "Admin" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapRazorPages();
});


app.Run();
void ConfigureServices(IServiceCollection services)
{
    // ????...

    services.AddTransient<EmailService>();
}
