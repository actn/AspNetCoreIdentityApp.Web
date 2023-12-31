using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection"));

});

builder.Services.AddIdentityWithExt();

builder.Services.ConfigureApplicationCookie(options =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "AppLoginCookie";
    options.ExpireTimeSpan = TimeSpan.FromDays(10);
    options.SlidingExpiration = true;
    options.Cookie = cookieBuilder;
    options.LoginPath = new PathString("/Home/SignIn");
    options.LogoutPath = new PathString("/Member/Logout");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
