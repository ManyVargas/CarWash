using Application.UsesCases.Usuarios.LoginUsuario;
using Core.Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebApplicationMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;

        public LoginController(HttpClient client)
        {
            _client = client;
        }
        [Route("admin/login")]
        [HttpGet]
        public IActionResult LoginFormView()
        {
            return View();
        }

        [Route("admin/login")]
        [HttpPost]
        public async Task<ActionResult<LoginUsuarioResponse>> LoginFormSubmit([FromForm] LoginUsuarioRequest loginUsuarioRequest)
        {
            var url = "https://localhost:7210/api/Usuario/login";
            try
            {
                // Serializar el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(loginUsuarioRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Enviar solicitud POST a la API
                var response = await _client.PostAsync(url, content);

                if(response.IsSuccessStatusCode)
                {
                    // Leer el contenido de la respuesta como JSON
                    var responseString = await response.Content.ReadAsStringAsync();

                    // Deserializar el JSON en un objeto de respuesta
                    var loginResponse = JsonConvert.DeserializeObject<LoginUsuarioResponse>(responseString);

                    HttpContext.Session.SetString("NombreUsuario", loginResponse.Nombre);
                    HttpContext.Session.SetString("RolUsuario", loginResponse.Rol);
                    HttpContext.Session.SetInt32("IdUsuario", loginResponse.UsuarioID);

                    // Retornar la respuesta a la vista o al cliente
                    return RedirectToAction("AdminHome", "Admin", new {id = loginResponse?.UsuarioID, nombre = loginResponse?.Nombre, rol = loginResponse?.Rol}); ;
                }
                else
                {
                    // Manejar errores de la API
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest(new { message = "Error en la autenticación", details = errorContent });
                }

            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                TempData["ToastType"] = "error";
                // Manejar excepciones
                return RedirectToAction("LoginFormView");
            }
            

        }
    }
}
