using Application.UsesCases.Servicios.ObtenerServicios;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using WebApplicationMVC.Models;

namespace WebApplicationMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<ActionResult<IEnumerable<ObtenerServiciosResponse>>> Index()
        {
            string url = "https://localhost:7210/api/Servicio/todosServicios";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var servicios = JsonConvert.DeserializeObject<List<ObtenerServiciosResponse>>(jsonResponse);

                    return View("~/Views/Home/Index.cshtml", servicios);
                }
                else
                {
                    // Maneja el caso de error
                    ViewData["Error"] = $"Error al obtener los servicios. Código: {response.StatusCode}";
                    return View("~/Views/Home/Index.cshtml", new List<ObtenerServiciosResponse>());
                }
            }
            catch (Exception ex)
            {
                // Maneja excepciones
                ViewData["Error"] = $"Ocurrió un error: {ex.Message}";
                return View("~/Views/Home/Index.cshtml", new List<ObtenerServiciosResponse>());

            }
        }

        public IActionResult Servicios()
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
