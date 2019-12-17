using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ResearchData.Portal.Controllers
{
    public class ErrosController : Controller
    {
        private readonly IStringLocalizer<ErrosController> _localizador;

        public ErrosController(IStringLocalizer<ErrosController> localizador)
        {
            _localizador = localizador;
        }

        [HttpGet]
        public IActionResult TratarCodigoDeErro(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.MensagemErro = "Ainda não temos essa pagina. Sorry!";
                    ViewBag.RouterOfException = statusCodeData.OriginalPath;
                    break;
                case 203:
                    ViewBag.MensagemErro = "Não permitimos acesso indevido a informações de terceiros. Sorry!";
                    ViewBag.RouterOfException = statusCodeData.OriginalPath;
                    break;
                default:
                    break;
            }

            return View();
        }
    }
}