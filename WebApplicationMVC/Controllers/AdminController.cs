using Application.UsesCases.Servicios.ObtenerServicios;
using Application.UsesCases.Servicios.RegistrarServicio;
using Application.UsesCases.Usuarios.LoginUsuario;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebApplicationMVC.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult AdminHome()
        {
            // Recuperar datos de la sesión
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            var rolUsuario = HttpContext.Session.GetString("RolUsuario");

            if (string.IsNullOrEmpty(nombreUsuario))
            {
                // Redirigir al login si no hay sesión activa
                return RedirectToAction("LoginFormView", "Login");
            }

            ViewData["Nombre"] = nombreUsuario;
            ViewData["Rol"] = rolUsuario;

            return View();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObtenerServiciosResponse>>> AdminServicios()
        {
            string url = "https://localhost:7210/api/Servicio/todosServicios";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if(response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var servicios = JsonConvert.DeserializeObject<List<ObtenerServiciosResponse>>(jsonResponse);

                    return View("~/Views/Admin/Servicios/AdminServicios.cshtml",servicios);
                }
                else
                {
                    // Maneja el caso de error
                    ViewData["Error"] = $"Error al obtener los servicios. Código: {response.StatusCode}";
                    return View("~/Views/Admin/Servicios/AdminServicios.cshtml", new List<ObtenerServiciosResponse>());
                }
            }
            catch (Exception ex)
            {
                // Maneja excepciones
                ViewData["Error"] = $"Ocurrió un error: {ex.Message}";
                return View("~/Views/Admin/Servicios/AdminServicios.cshtml",new List<ObtenerServiciosResponse>());

            }
        }

        [HttpGet]
        public IActionResult RegistrarServicio()
        {
            return View("~/Views/Admin/Servicios/RegistrarServicio.cshtml");
        }

        [HttpPost]
        [Route("Admin/RegistrarServicioSubmit")]
        public async Task<IActionResult> RegistrarServicioSubmit([FromForm]RegistrarServicioRequest registrarServicioRequest)
        {

            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Por favor, complete todos los campos requeridos.";
                TempData["ToastType"] = "error";
                return View("~/Views/Admin/Servicios/RegistrarServicio.cshtml", registrarServicioRequest);
            }

            string url = "https://localhost:7210/api/Servicio/agregar";

           

            try
            {
                // Serializar el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(registrarServicioRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Enviar solicitud POST a la API
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    
                    TempData["ToastMessage"] = "Servicio agregado correctamente.";
                    TempData["ToastType"] = "success";

                    // Retornar la respuesta a la vista o al cliente
                    return RedirectToAction("AdminServicios");
                }
                else
                {
                    
                    TempData["ToastMessage"] = "Error al agregar el servicio. Intente nuevamente.";
                    TempData["ToastType"] = "error";

                    return View("~/Views/Admin/Servicios/RegistrarServicio.cshtml", registrarServicioRequest);
                }

            }
            catch (Exception ex)
            {
                // Guardar mensaje de excepción en TempData
                TempData["ToastMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                TempData["ToastType"] = "error";

                return View("~/Views/Admin/Servicios/RegistrarServicio.cshtml", registrarServicioRequest);
            }
        }
    }
}
