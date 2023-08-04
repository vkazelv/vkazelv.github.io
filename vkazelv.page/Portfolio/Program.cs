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

#region [DB Connection 연결설정]

#endregion


/// <summary>
/// 외부 인증 관련 ( 참고 : https://learn.microsoft.com/ko-kr/aspnet/core/security/authentication/social/google-logins?view=aspnetcore-7.0)
/// 공유 인증 cookie s without ASP.NET Core Identity: (https://docs.microsoft.com/ko-kr/aspnet/core/security/cookie-sharing?view=aspnetcore-6.0)
/// sample code : https://www.roundthecode.com/dotnet/how-to-add-google-authentication-to-a-asp-net-core-application 
/// nuget 추가  Microsoft.AspNetCore.Authentication.Google
/// </summary>
services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = KakaoTalkAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    // 승인된 리디렉션 URI 등록 
    //options.LoginPath = "/account/google-login";
    options.LoginPath = "/account/login-kakaotalk";

    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Forbidden/";
}).AddGoogle(options =>
{
    // Google Cloud API서비스 OAuth 2.0 클라이언트 ID 등록 , key 발급 
    // key 는 secret 저장소에 저장 
    var clientInfo = OAuthConfig.GetClientInfo(GoogleDefaults.AuthenticationScheme);
    options.ClientId = clientInfo.clientID;
    options.ClientSecret = clientInfo.clientSecret;
}).AddKakaoTalk(options => {
    /// Microsoft.AspNetCore.Authentication.Google 확장  
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
app.UseAuthentication(); // 인증 활성화 
app.UseAuthorization(); // 권한부여 활성화 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
