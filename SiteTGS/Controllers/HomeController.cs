using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiteTGS.Models;
using SiteTGS.Services;

namespace SiteTGS.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMailService mailService;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }

        public HomeController(ILogger<HomeController> logger, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
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
                return Error();
            }

        }

        [HttpPost]
        public JsonResult ValidaLogin(LoginRequest req)
        {
            string login = "", senha = "";
            if (req != null)
            {
                login = req.Login == null ? "" : req.Login;
                senha = req.Senha == null ? "" : req.Senha;
            }
            List<DownloadResponse> lst = new List<DownloadResponse>();
            login = login.ToUpper();
            senha = senha.ToUpper();
            if (((login == "RJSUPORTE" || login == "UNIMATOS" || login == "MATOS") && senha == "240862")
                    ||
                (login == "CLIENTES" && senha == "MATOS2015"))
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                string path = "";
                path = webRootPath + "\\Arquivos\\";
                DirectoryInfo directory = new DirectoryInfo(path);
                foreach (var item in directory.GetFiles())
                {
                    string nome = item.Name;
                    string size = BytesToString(item.Length);
                    string data = item.LastWriteTime.ToString("dd/MM/yyyy");
                    DateTime dt = item.LastWriteTime;
                    lst.Add(new DownloadResponse() { Data = data, Tamanho = size, Nome = nome, DataDT = dt });
                }
                lst.Sort((x, y) => y.DataDT.CompareTo(x.DataDT));
            }
            return Json(JsonConvert.SerializeObject(lst));
        }

        [HttpGet("Home/DownloadFile/{FileName}")]
        public IActionResult DownloadFile(string FileName)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                string path = webRootPath + "\\Arquivos\\" + FileName;

                byte[] bytes = System.IO.File.ReadAllBytes(path);

                return File(bytes, "application/octet-stream", FileName);
            }
            catch (Exception ex)
            {
                return NotFound();
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
