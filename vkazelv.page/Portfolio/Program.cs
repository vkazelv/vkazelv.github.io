using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Logging;
using RureuLib.Configs;
using RureuLib.OAuth.KakaoTalk;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

#region [DB Connection ���ἳ��]

#endregion


/// <summary>
/// �ܺ� ���� ���� ( ���� : https://learn.microsoft.com/ko-kr/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-7.0)
/// ���� ���� cookie s without ASP.NET Core Identity: (https://docs.microsoft.com/ko-kr/aspnet/core/security/cookie-sharing?view=aspnetcore-6.0)
/// sample code : https://www.roundthecode.com/dotnet/how-to-add-google-authentication-to-a-asp-net-core-application 
/// nuget �߰�  Microsoft.AspNetCore.Authentication.Google
/// </summary>
services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = KakaoTalkAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    // ���ε� ���𷺼� URI ��� 
    //options.LoginPath = "/account/google-login";
    options.LoginPath = "/account/login-kakaotalk";

    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Forbidden/";
}).AddGoogle(options =>
{
    // Google Cloud API���� OAuth 2.0 Ŭ���̾�Ʈ ID ��� , key �߱� 
    // key �� secret ����ҿ� ���� 
    var clientInfo = OAuthConfig.GetClientInfo(GoogleDefaults.AuthenticationScheme);
    options.ClientId = clientInfo.clientID;
    options.ClientSecret = clientInfo.clientSecret;
}).AddKakaoTalk(options => {
    /// Microsoft.AspNetCore.Authentication.Google Ȯ��  
    var clientInfo = OAuthConfig.GetClientInfo(KakaoTalkAuthenticationDefaults.AuthenticationScheme);
    options.ClientId = clientInfo.clientID;
    options.ClientSecret = clientInfo.clientSecret;
    options.SaveTokens = true; 
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
});
app.UseAuthentication(); // ���� Ȱ��ȭ 
app.UseAuthorization(); // ���Ѻο� Ȱ��ȭ 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
