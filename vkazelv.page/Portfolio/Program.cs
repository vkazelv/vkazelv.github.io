using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

#region [DB Connection 연결설정]

#endregion


/// <summary>
/// 외부 인증 관련 ( 참고 : https://learn.microsoft.com/ko-kr/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-7.0)
/// 공유 인증 cookie s without ASP.NET Core Identity: (https://docs.microsoft.com/ko-kr/aspnet/core/security/cookie-sharing?view=aspnetcore-6.0)
/// nuget 추가  Microsoft.AspNetCore.Authentication.Google
/// </summary>
services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/account/google-login";
}).AddGoogle(googleOptions =>
{
    googleOptions.ClientId = RureuLib.Configs.GoogleConfig.ClientID;
    googleOptions.ClientSecret = RureuLib.Configs.GoogleConfig.ClientSecret;
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
