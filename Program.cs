using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DepoYonetimSistemi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Session Configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum s�resi
    options.Cookie.HttpOnly = true; // G�venlik i�in sadece HTTP �zerinden eri�im
    options.Cookie.IsEssential = true; // GDPR ve benzeri d�zenlemeler i�in zorunlu
});

// Add Authentication and Cookie Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Genel/GirisSayfasi"; // Giri� sayfas�
        options.AccessDeniedPath = "/Genel/YetkisizErisim"; // Yetkisiz eri�im sayfas�
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // �erez s�resi
        options.SlidingExpiration = true; // Otomatik �erez yenileme
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add Database Context
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29))));

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
    pattern: "{controller=Genel}/{action=GirisSayfasi}/{id?}");

app.Run();
