using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google; 
using RureuLib.OAuth.KakaoTalk; 

namespace Portfolio.Controllers
{
    [AllowAnonymous, Route("account")]
    public class AccountController : Controller
    { 
        #region [Google OAuth Login] 
        /// <summary>
        /// 로그인요청 
        /// </summary>
        /// <returns></returns>
        [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        /// <summary>
        /// 로그인 후 콜백 
        /// </summary>
        /// <returns></returns>
        [Route("auth/google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

   /*         var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });*/

            //return Json(claims);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region [Kakao OAuth2.0 Login]  
        /// <summary>
        /// 로그인요청 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [Route("kakaotalk")]
        public IActionResult KakaoLogin([FromForm] string provider)
        {
            ///IsPersistent 쿠키 유지 설정 
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("KakaoTalkResponse") , IsPersistent = true };

            return Challenge(properties, KakaoTalkAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// 로그인 후 콜백 
        /// </summary>
        /// <returns></returns>
        [Route("auth/kakaotalk")]
        public async Task<IActionResult> KakaoTalkResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //var claims = result.Principal.Identities.FirstOrDefault().Claims;
             
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
