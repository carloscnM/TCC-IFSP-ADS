using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ResearchData.Portal.Controllers
{
    public class SobreController : Controller
    {
        public IActionResult TermoDeUso()
        {
            return PartialView();
        }

        public IActionResult Sobre()
        {
            return View();
        }
    }
}