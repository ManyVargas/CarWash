using Application.UsesCases.Usuarios.ObtenerUsuario;
using Microsoft.AspNetCore.Mvc;
using MvcWebClient.Models;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace MvcWebClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly HttpClient _httpClient;

        public LoginController(ILogger<LoginController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        

        public async Task<IActionResult> Admin()
        {
            string apiUrl = "https://localhost:7210/api/Usuario/todosUsuarios";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                //Deserializar la respuesta obtenida del api
                var jsonString = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora mayúsculas/minúsculas en los nombres de las propiedades
                };
                var usuarios = JsonSerializer.Deserialize<IEnumerable<ObtenerUsuariosResponse>>(jsonString, opciones);


                //retornar a la vista los usuarios
                return View("~/Views/Admin/index.cshtml", usuarios);
            }

            return View();
            //return View("~/Views/Admin/index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
