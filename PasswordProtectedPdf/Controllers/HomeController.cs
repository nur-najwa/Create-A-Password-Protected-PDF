using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using PasswordProtectedPdf.Models;
using System.Diagnostics;

namespace PasswordProtectedPdf.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult OpenPDF()
        {
            var pdfilePath = _configuration.GetSection("File").GetSection("Path").Value;

            byte[] bytes = System.IO.File.ReadAllBytes(pdfilePath);
            using(MemoryStream streamInput =new MemoryStream(bytes))
            {
                using (MemoryStream streamOutput = new MemoryStream())
                {
                    string pdfFilePassword = "najwa1234";
                    PdfReader reader = new PdfReader(streamInput);
                    PdfReader.unethicalreading = true;
                    PdfEncryptor.Encrypt(reader,streamOutput,true,pdfFilePassword,pdfFilePassword,PdfWriter.ALLOW_SCREENREADERS);
                    bytes=streamOutput.ToArray();
                    return File(bytes, "application/pdf");
                }
            }
        }
    }
}