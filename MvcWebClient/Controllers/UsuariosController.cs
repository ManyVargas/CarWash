using Application.UsesCases.Usuarios.ObtenerUsuario;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MvcWebClient.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuariosController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
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
                var usuarios = JsonSerializer.Deserialize<IEnumerable<ObtenerUsuariosResponse>>(jsonString,opciones);

                //retornar a la vista los usuarios
                return View(usuarios);
            }

            return View("Error");
        }
    }
}
