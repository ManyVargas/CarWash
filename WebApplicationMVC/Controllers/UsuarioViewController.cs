
using Application.UsesCases.Servicios.ActualizarServicio;
using Application.UsesCases.Servicios.RegistrarServicio;
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.ObtenerUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace WebApplicationMVC.Controllers
{
    public class UsuarioViewController : Controller
    {
        private readonly HttpClient _httpClient;

        public UsuarioViewController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObtenerUsuariosResponse>>> AdminUsuarios()
        {
            string url = "https://localhost:7210/api/Usuario/todosUsuarios";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var usuarios = JsonConvert.DeserializeObject<List<ObtenerUsuariosResponse>>(jsonResponse);

                    return View("~/Views/Admin/Usuarios/AdminUsuarios.cshtml", usuarios);
                }
                else
                {
                    // Maneja el caso de error
                    ViewData["Error"] = $"Error al obtener los servicios. Código: {response.StatusCode}";
                    return View("~/Views/Admin/Usuarios/AdminUsuarios.cshtml", new List<ObtenerUsuariosResponse>());
                }
            }
            catch (Exception ex)
            {
                // Maneja excepciones
                ViewData["Error"] = $"Ocurrió un error: {ex.Message}";
                return View("~/Views/Admin/Usuarios/AdminUsuarios.cshtml", new List<ObtenerUsuariosResponse>());

            }
        }

        [HttpGet]
        public IActionResult RegistrarUsuario()
        {
            return View("~/Views/Admin/Usuarios/RegistrarUsuario.cshtml");
        }

        [HttpPost]
        [Route("Admin/RegistrarUsuarioSubmit")]
        public async Task<IActionResult> RegistrarUsuarioSubmit([FromForm] RegistrarUsuarioRequest registrarUsuarioRequest)
        {

            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Por favor, complete todos los campos requeridos.";
                TempData["ToastType"] = "error";
                return View("~/Views/Admin/Usuarios/RegistrarUsuario.cshtml", registrarUsuarioRequest);
            }

            string url = "https://localhost:7210/api/Usuario/agregar";



            try
            {
                // Serializar el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(registrarUsuarioRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Enviar solicitud POST a la API
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {

                    TempData["ToastMessage"] = "Usuario agregado correctamente.";
                    TempData["ToastType"] = "success";

                    // Retornar la respuesta a la vista o al cliente
                    return RedirectToAction("AdminUsuarios", "UsuarioView");
                }
                else
                {

                    TempData["ToastMessage"] = "Error al agregar el usuario. Intente nuevamente.";
                    TempData["ToastType"] = "error";

                    return View("~/Views/Admin/Usuarios/RegistrarUsuario.cshtml", registrarUsuarioRequest);
                }

            }
            catch (Exception ex)
            {
                // Guardar mensaje de excepción en TempData
                TempData["ToastMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                TempData["ToastType"] = "error";

                return View("~/Views/Admin/Usuarios/RegistrarUsuario.cshtml", registrarUsuarioRequest);
            }
        }

        [HttpGet]
        [Route("Admin/EliminarUsuario")]
        public async Task<IActionResult> EliminarUsuario(string? email, string? telefono)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(telefono))
            {
                TempData["ToastMessage"] = "Debe proporcionar un email o teléfono válido.";
                TempData["ToastType"] = "error";
                return RedirectToAction("AdminUsuarios");
            }

            string url = $"https://localhost:7210/api/Usuario/eliminar?email={Uri.EscapeDataString(email)}&telefono={Uri.EscapeDataString(telefono)}";

            try
            {
                var eliminarUsuarioRequest = new EliminarUsuarioRequest { Email = email, Telefono = telefono };

                var jsonContent = JsonConvert.SerializeObject(eliminarUsuarioRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = $"El usuario con {(email != null ? "email" : "telefono")} {(email ?? telefono)} se elimino correctamente.";
                    TempData["ToastType"] = "success";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ToastMessage"] = $"No se pudo eliminar el usuario. Detalles: {errorContent}";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error al intentar eliminar el usuario: {ex.Message}";
                TempData["ToastType"] = "error";
            }


            return RedirectToAction("AdminUsuarios");
        }

        public IActionResult ActualizarUsuarioForm()
        {
            return View("~/Views/Admin/Usuarios/ActualizarUsuarioForm.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarUsuario(string? email, string? telefono)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(telefono))
            {
                TempData["ToastMessage"] = "Debe proporcionar un email o teléfono válido.";
                TempData["ToastType"] = "error";
                return RedirectToAction("AdminUsuarios", "Admin");
            }

            string url = $"https://localhost:7210/api/Usuario/buscar?email={Uri.EscapeDataString(email ?? "")}&telefono={Uri.EscapeDataString(telefono ?? "")}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var usuario = JsonConvert.DeserializeObject<ActualizarUsuarioRequest>(jsonResponse);

                    // Envía el modelo del usuario a la vista
                    return View("~/Views/Admin/Usuarios/ActualizarUsuarioForm.cshtml", usuario);
                }
                else
                {
                    TempData["ToastMessage"] = $"No se pudo obtener el usuario con {(email ?? telefono)}.";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar obtener el usuario: {ex.Message}";
                TempData["ToastType"] = "error";
            }

            return RedirectToAction("AdminUsuarios");
        }



        [HttpPost]
        [Route("Admin/ActualizarUsuario")]
        public async Task<IActionResult> ActualizarUsuarioSubmit(ActualizarUsuarioRequest actualizarUsuarioRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Por favor, complete todos los campos requeridos.";
                TempData["ToastType"] = "error";
                return View("~/Views/Admin/Usuarios/ActualizarUsuarioForm.cshtml", actualizarUsuarioRequest);
            }

            string url = $"https://localhost:7210/api/Usuario/actualizar?email={actualizarUsuarioRequest.Email}&telefono={actualizarUsuarioRequest.Telefono}";

            try
            {
                var jsonContent = JsonConvert.SerializeObject(actualizarUsuarioRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "Usuario actualizado correctamente.";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("AdminUsuarios");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ToastMessage"] = $"No se pudo actualizar el usuario. Detalles: {errorContent}";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar actualizar el usuario: {ex.Message}";
                TempData["ToastType"] = "error";
            }

            return View("~/Views/Admin/Usuarios/ActualizarUsuarioForm.cshtml", actualizarUsuarioRequest);
        }
    }
}
