using Application.UsesCases.Clientes.ActualizarCliente;
using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Clientes.ObtenerClientes;
using Application.UsesCases.Clientes.RegistrarCliente;
using Application.UsesCases.Usuarios.ActualizarUsuario;
using Application.UsesCases.Usuarios.EliminarUsuario;
using Application.UsesCases.Usuarios.RegistrarUsuario;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebApplicationMVC.Controllers
{
    public class ClienteViewController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClienteViewController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObtenerClientesResponse>>> AdminClientes()
        {
            string url = "https://localhost:7210/api/Cliente/todosClientes";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var clientes = JsonConvert.DeserializeObject<List<ObtenerClientesResponse>>(jsonResponse);

                    return View("~/Views/Admin/Clientes/AdminClientes.cshtml", clientes);
                }
                else
                {
                    // Maneja el caso de error
                    ViewData["Error"] = $"Error al obtener los servicios. Código: {response.StatusCode}";
                    return View("~/Views/Admin/Clientes/AdminClientes.cshtml", new List<ObtenerClientesResponse>());
                }
            }
            catch (Exception ex)
            {
                // Maneja excepciones
                ViewData["Error"] = $"Ocurrió un error: {ex.Message}";
                return View("~/Views/Admin/Clientes/AdminClientes.cshtml", new List<ObtenerClientesResponse>());

            }
        }

        [HttpGet]
        public IActionResult RegistrarCliente()
        {
            return View("~/Views/Admin/Clientes/RegistrarCliente.cshtml");
        }

        [HttpPost]
        [Route("Admin/RegistrarClienteSubmit")]
        public async Task<IActionResult> RegistrarClienteSubmit([FromForm] RegistrarClienteRequest registrarClienteRequest)
        {

            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Por favor, complete todos los campos requeridos.";
                TempData["ToastType"] = "error";
                return View("~/Views/Admin/Clientes/RegistrarCliente.cshtml", registrarClienteRequest);
            }

            string url = "https://localhost:7210/api/Cliente/agregar";



            try
            {
                // Serializar el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(registrarClienteRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Enviar solicitud POST a la API
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {

                    TempData["ToastMessage"] = "Cliente agregado correctamente.";
                    TempData["ToastType"] = "success";

                    // Retornar la respuesta a la vista o al cliente
                    return RedirectToAction("AdminClientes");
                }
                else
                {

                    TempData["ToastMessage"] = "Error al agregar el cliente. Intente nuevamente.";
                    TempData["ToastType"] = "error";

                    return View("~/Views/Admin/Clientes/RegistrarCliente.cshtml", registrarClienteRequest);
                }

            }
            catch (Exception ex)
            {
                // Guardar mensaje de excepción en TempData
                TempData["ToastMessage"] = $"Ocurrió un error inesperado: {ex.Message}";
                TempData["ToastType"] = "error";

                return View("~/Views/Admin/Clientes/RegistrarCliente.cshtml", registrarClienteRequest);
            }
        }

        public IActionResult ActualizarClienteForm()
        {
            return View("~/Views/Admin/Clientes/ActualizarClienteForm.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarCliente(string? email, string? telefono)
        {
            if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(telefono))
            {
                TempData["ToastMessage"] = "Debe proporcionar un email o teléfono válido.";
                TempData["ToastType"] = "error";
                return RedirectToAction("AdminClientes");
            }

            string url = $"https://localhost:7210/api/Cliente/buscar?email={Uri.EscapeDataString(email ?? "")}&telefono={Uri.EscapeDataString(telefono ?? "")}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var cliente = JsonConvert.DeserializeObject<ActualizarClienteRequest>(jsonResponse);

                    // Envía el modelo del usuario a la vista
                    return View("~/Views/Admin/Clientes/ActualizarClienteForm.cshtml", cliente);
                }
                else
                {
                    TempData["ToastMessage"] = $"No se pudo obtener el cliente con {(email ?? telefono)}.";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar obtener el cliente: {ex.Message}";
                TempData["ToastType"] = "error";
            }

            return RedirectToAction("AdminClientes");
        }



        [HttpPost]
        [Route("Admin/ActualizarCliente")]
        public async Task<IActionResult> ActualizarClienteSubmit(ActualizarClienteRequest actualizarClienteRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Por favor, complete todos los campos requeridos.";
                TempData["ToastType"] = "error";
                return View("~/Views/Admin/Clientes/ActualizarClienteForm.cshtml", actualizarClienteRequest);
            }

            string url = $"https://localhost:7210/api/Cliente/actualizar?email={actualizarClienteRequest.Email}&telefono={actualizarClienteRequest.Telefono}";

            try
            {
                var jsonContent = JsonConvert.SerializeObject(actualizarClienteRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "Cliente actualizado correctamente.";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("AdminClientes");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ToastMessage"] = $"No se pudo actualizar el cliente. Detalles: {errorContent}";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar actualizar el cliente: {ex.Message}";
                TempData["ToastType"] = "error";
            }

            return View("~/Views/Admin/Clientes/ActualizarClienteForm.cshtml", actualizarClienteRequest);
        }


        [HttpGet]
        [Route("Admin/EliminarCliente")]
        public async Task<IActionResult> EliminarCliente(string? email, string? telefono)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(telefono))
            {
                TempData["ToastMessage"] = "Debe proporcionar un email o teléfono válido.";
                TempData["ToastType"] = "error";
                return RedirectToAction("AdminClientes");
            }

            string url = $"https://localhost:7210/api/Cliente/eliminar?email={Uri.EscapeDataString(email)}&telefono={Uri.EscapeDataString(telefono)}";

            try
            {
                var eliminarClienteRequest = new EliminarClienteRequest { Email = email, Telefono = telefono };

                var jsonContent = JsonConvert.SerializeObject(eliminarClienteRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = $"El cliente con {(email != null ? "email" : "telefono")} {(email ?? telefono)} se elimino correctamente.";
                    TempData["ToastType"] = "success";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ToastMessage"] = $"No se pudo eliminar el cliente. Detalles: {errorContent}";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error al intentar eliminar el cliente: {ex.Message}";
                TempData["ToastType"] = "error";
            }


            return RedirectToAction("AdminClientes");
        }
    }
}
