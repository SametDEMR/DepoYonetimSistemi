var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Authentication and Cookie Configuration
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Genel/GirisSayfasi"; // Giri� sayfas�
        options.AccessDeniedPath = "/Genel/YetkisizErisim"; // Yetkisiz eri�im sayfas�
    });

builder.Services.AddAuthorization();

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

app.UseAuthentication(); // Authentication middleware EKLEND�
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Genel}/{action=GirisSayfasi}/{id?}");

app.Run();
