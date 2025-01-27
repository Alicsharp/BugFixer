using BugFixer.Application;
using BugFixer.infrastructure;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Encodings.Web;
using System.Text.Unicode;
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
#region Services
builder.Services.AddControllersWithViews();
builder.Services.ApplicationConfig(builder.Configuration);
builder.Services.Config(builder.Configuration.GetConnectionString("DefaultConnection")) ;
builder.Services.AddHttpClient<ICaptchaValidator,GoogleReCaptchaValidator>();
//بررسی شود
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create
    (allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});
#endregion

#region MiddlerWares

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


app.UseEndpoints(endpoints =>
{
    // مسیر پیش‌فرض برای Area
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    // مسیر پیش‌فرض برای بخش‌های غیر Area
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
#endregion