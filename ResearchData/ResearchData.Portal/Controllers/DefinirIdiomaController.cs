using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ResearchData.Portal.Controllers
{
    public class DefinirIdiomaController : Controller
    {
        [HttpPost]
        public IActionResult SelecionarLinguagem(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                }
            );

            var returnUrl = HttpContext.Request.Headers["Referer"].ToString();
            return Redirect(returnUrl);
        }
    }
}