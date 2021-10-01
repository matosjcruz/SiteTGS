using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiteTGS.Models;
using SiteTGS.Services;

namespace SiteTGS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailService mailService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
        }
        public IActionResult Contato()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromForm] FormMailRequest request)
        {
            try
            {
                MailRequest mr = new MailRequest();
                mr.Body = "Nome: "+request.Nome+" Telefone:"+request.Telefone+Environment.NewLine+"Email: "+request.Email+Environment.NewLine+"Mensagem: "+request.Mensagem;
                mr.Subject = "Contato pelo site TGS Sistemas";
                mr.ToEmail = "jessica@tgssistemas.com;contato@tgssistemas.com;marciano@tgssistemas.com;marcianoandradejr@gmail.com";
                await mailService.SendEmailAsync(mr);
                return Json("Enviado com sucesso");
            }
            catch (Exception ex)
            {
                return Json("Ocorreu um erro");
            }

        }

        public IActionResult Solucoes(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            else
            {
                ViewBag.id = "";
            }
            return View();
        }

        public IActionResult Representantes()
        {
            return View();
        }

        public IActionResult Suporte()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
