using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153501_Kosach.Controllers
{
    public class IdentityController : Controller
    {
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
                "oidc",
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                });
        }


        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(
                "oidc",
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home")
                });
        }
    }
}
