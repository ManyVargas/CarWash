using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace MvcWebClient.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClientesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            string apiUrl = "https://localhost:7210/api/Cliente/todosClientes";

            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                //Deserializar la respuesta obtenida del api
                var jsonString = await response.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Ignora mayúsculas/minúsculas en los nombres de las propiedades
                };
                var clientes = JsonSerializer.Deserialize<IEnumerable<ObtenerClientesResponse>>(jsonString, opciones);
                return View(clientes);
            }
            return View("Error");
        }
    }
}
