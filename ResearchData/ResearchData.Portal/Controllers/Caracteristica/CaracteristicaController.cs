using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchData.Portal.Data.Repositorio.Caracteristicas;
using ResearchData.Portal.Models.Negocio.Sujeitos;
using ResearchData.Portal.Models.ViewModels.Caracteristicas;
using Microsoft.Extensions.Localization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;

namespace ResearchData.Portal.Controllers.Caracteristicas
{
    [Authorize]
    public class CaracteristicaController : Controller
    {
        private readonly IStringLocalizer<CaracteristicaController> _localizador;
        private ICaracteristicaRepositorio _caraRepo;
        IHostingEnvironment _appEnvironment;

        public CaracteristicaController(ICaracteristicaRepositorio caracRepo, IStringLocalizer<CaracteristicaController> localizador, IHostingEnvironment env)
        {
            _caraRepo = caracRepo;
            _localizador = localizador;
            _appEnvironment = env;
        }
        public IActionResult CriarCaracteristica(bool? msg)
        {
            if (msg != null)
            {
                ViewBag.CadatroSucesso = true;
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarCaracteristica(CriarCaracteristicaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                if(!_caraRepo.ExisteCaracteristica(modelo.Descricao, modelo.TipoDoDado, modelo.Comun))
                {
                    Caracteristica caracteristica = new Caracteristica()
                    {
                        Descricao = modelo.Descricao,
                        TipoDoDado = modelo.TipoDoDado,
                        Comun = Convert.ToBoolean(modelo.Comun)
                    };

                    var resultado = await _caraRepo.AddCaracteristica(caracteristica);
                    return RedirectToAction("CriarCaracteristica", "Caracteristica", new { msg = true });
                }
                ModelState.AddModelError("", _localizador["The description feature"] + $" {modelo.Descricao}, "+ _localizador[" and type "] + $" {modelo.TipoDoDado.GetDisplayNameGlobal()}" + _localizador[" already registered in our base!"]);
            }
            ViewBag.CadatroSucesso = false;
            return View(modelo);
        }


        public IActionResult ConsultarCaracteristicas(int tipo)
        {
            bool tipobusca = true;

            if (tipo == 2)
                tipobusca = false;

            IList<ListaDeCaracteristicasViewModel> modelo;
            modelo = _caraRepo.ListarTodasCaracterisca(tipobusca);
            ViewBag.Tipo = tipobusca;
            return View(modelo);
        }

        public IActionResult ImportarCaracteristicas()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportarCaracteristicas(ImportarCaracteristicaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                if (modelo.ArquivoXml.ContentType.Equals("text/xml"))
                {
                    try
                    {
                        var xmlSerializer = new XmlSerializer(typeof(ListaCaracteristicaViewModel));
                        ListaCaracteristicaViewModel caracteristicas;
                        using (var fileStream = modelo.ArquivoXml.OpenReadStream())
                        {
                            caracteristicas = (ListaCaracteristicaViewModel)xmlSerializer.Deserialize(fileStream);
                        }
                        if(caracteristicas.Caracteristicas.Count() > 0)
                        {
                            modelo.Log = new List<LogImportacaoViewModel>();
                            LogImportacaoViewModel logProcessamento;
                            foreach (var item in caracteristicas.Caracteristicas)
                            {
                                logProcessamento = new LogImportacaoViewModel();

                                bool teste = _caraRepo.ExisteCaracteristica(item.Descricao, item.TipoDoDado, item.Comun);

                                if (!_caraRepo.ExisteCaracteristica(item.Descricao, item.TipoDoDado, item.Comun))
                                {
                                    
                                    bool resultado = await _caraRepo.AddCaracteristica(item).ConfigureAwait(false);
                                    if (resultado)
                                    {
                                        logProcessamento.Msg = _localizador["The description feature"] + $" {item.Descricao} "+  _localizador["was successfully registered"];
                                        logProcessamento.Tipo = "green";
                                        modelo.Log.Add(logProcessamento);
                                    }
                                    else
                                    {
                                        logProcessamento.Msg = _localizador["Unable to register feature"] + $"{item.Descricao}";
                                        logProcessamento.Tipo = "red";
                                        modelo.Log.Add(logProcessamento);
                                    }
                                }
                                else
                                {
                                    var tipo = "comum";

                                    if (!item.Comun)
                                        tipo = "Especifica";


                                    logProcessamento.Msg = _localizador["The Feature"] + $" { tipo} {item.Descricao} "+ _localizador["of given type:"] + $" {item.TipoDoDado.GetDisplayNameGlobal()}, "+ _localizador["already exists!"];
                                    logProcessamento.Tipo = "red";
                                    modelo.Log.Add(logProcessamento);
                                }

                            }
                            return View(modelo);
                        }
                        ModelState.AddModelError("", _localizador["There are no features to import."]);
                        return View();
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", _localizador["Unable to import data -"] + $"{ e.Message}");
                        return View();
                    }
                }
                ModelState.AddModelError("", _localizador["This is not an XML file, please select xml file as default available for download"]);
                return View();
            }
            ModelState.AddModelError("", _localizador["Need to select a valid file"]);
            return View();
        }


        public FileResult BaixarModelo()
        {
            var fs = System.IO.File.OpenRead("wwwroot/Arquivo/modeloImportCaracateristica.xml");
            return File(fs, "text/xml", "ModeloImportacao.xml");
        }
    }
}