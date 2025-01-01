using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Data;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session Configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Session timeout duration
    options.Cookie.HttpOnly = true; // Restrict access to cookies via HTTP only
    options.Cookie.IsEssential = true; // Mark as essential for GDPR compliance
});

// Add Authentication and Cookie Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Genel/GirisSayfasi"; // Login page path
        options.AccessDeniedPath = "/Genel/YetkisizErisim"; // Access denied page path
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Cookie expiration duration
        options.SlidingExpiration = true; // Enable sliding expiration
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29))));

// Add MemoryCache Service
builder.Services.AddMemoryCache();

// Add HTTP Client for API Integration
builder.Services.AddHttpClient("UrunlerApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // API base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Add session middleware
app.UseSession();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Genel}/{action=KarsilamaEkrani}/{id?}");

app.Run();