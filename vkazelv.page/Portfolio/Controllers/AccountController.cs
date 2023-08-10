using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google; 
using RureuLib.OAuth.KakaoTalk;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using System.Net;
using System.Text.Json;
using Portfolio.Models.KakaoPay;

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


        public async Task<IActionResult> KakaoPayAsync()
        {

            string url = "https://kapi.kakao.com/v1/payment/ready";
            string authorization = "f4a2ec54ca4e48690137cb6e1a0f3219";

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("KakaoAK", authorization);


            Dictionary<string, string> reqParams = new Dictionary<string, string>();
            reqParams.Add("cid", "TC0ONETIME"); // 가맹점 코드, 10자
            //reqParams.Add("cid_secret", "");
            reqParams.Add("partner_order_id", "testorder1");
            reqParams.Add("partner_user_id", "test1");
            reqParams.Add("item_name", "test");
            //reqParams.Add("item_code", "");
            reqParams.Add("quantity", "1");
            reqParams.Add("total_amount", "100");
            reqParams.Add("tax_free_amount", "10");
            //reqParams.Add("vat_amount", "");
            //reqParams.Add("green_deposit", "");
            reqParams.Add("approval_url", "https://localhost:7075/");
            reqParams.Add("cancel_url", "https://localhost:7075/");
            reqParams.Add("fail_url", "https://localhost:7075/");
            //reqParams.Add("available_cards", "");
            //reqParams.Add("payment_method_type", "");
            //reqParams.Add("install_month", "");
            //reqParams.Add("custom_json", "");



          //  StringContent scData = new(System.Text.Json.JsonSerializer.Serialize(reqParams), Encoding.UTF8, "application/x-www-form-urlencoded");

            using HttpResponseMessage response = await httpClient.PostAsync(url, new FormUrlEncodedContent(reqParams));


            if (response.IsSuccessStatusCode)
            { 
                string resContent = await response.Content.ReadAsStringAsync();
                if (resContent != null)
                {
                    /// json string.
                    string tag  = HttpUtility.UrlDecode(resContent, Encoding.UTF8);
                    ReadyRsp obj = JsonSerializer.Deserialize<ReadyRsp>(tag);

                    return Redirect(obj.next_redirect_pc_url);
                }
            }

            return View();
        }
        #endregion
    }
}
