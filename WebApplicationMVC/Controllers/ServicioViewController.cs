using Application.UsesCases.Clientes.EliminarCliente;
using Application.UsesCases.Servicios.ActualizarServicio;
using Application.UsesCases.Servicios.EliminarServicio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace WebApplicationMVC.Controllers
{
    public class ServicioViewController : Controller
    {
        private readonly HttpClient _httpClient;

        public ServicioViewController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("Admin/EliminarServicio/{nombre}")]
        public async Task<IActionResult> EliminarServicio(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                TempData["ToastMessage"] = "El nombre del servicio no es válido.";
                TempData["ToastType"] = "error";
                return RedirectToAction("AdminServicios","Admin");
            }

            string url = $"https://localhost:7210/api/Servicio/eliminar/{nombre}";

            try
            {
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = $"El servicio {nombre} se eliminó correctamente.";
                    TempData["ToastType"] = "success";
                }
                else
                {
                    TempData["ToastMessage"] = $"No se pudo eliminar el servicio {nombre}.";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar eliminar el servicio: {ex.Message}";
                TempData["ToastType"] = "error";
            }


            return RedirectToAction("AdminServicios", "Admin");
        }

        public IActionResult ActualizarServicioForm()
        {
            return View("~/Views/Admin/Servicios/ActualizarServicioForm.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarServicio(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                TempData["ToastMessage"] = "Debe proporcionar un nombre válido.";
                TempData["ToastType"] = "error";
                return RedirectToAction("AdminServicios", "Admin");
            }

            string url = $"https://localhost:7210/api/Servicio/{nombre}";

            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var servicio = JsonConvert.DeserializeObject<ActualizarServicioRequest>(jsonResponse);

                    // Envía el modelo del servicio a la vista
                    return View("~/Views/Admin/Servicios/ActualizarServicioForm.cshtml", servicio);
                }
                else
                {
                    TempData["ToastMessage"] = $"No se pudo obtener el servicio {nombre}.";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar obtener el servicio: {ex.Message}";
                TempData["ToastType"] = "error";
            }

            return RedirectToAction("AdminServicios", "Admin");

        }

        [HttpPost]
        [Route("Admin/ActualizarServicio")]
        public async Task<IActionResult> ActualizarServicioSubmit(ActualizarServicioRequest actualizarServicioRequest)
        {
            if (!ModelState.IsValid)
            {
                TempData["ToastMessage"] = "Por favor, complete todos los campos requeridos.";
                TempData["ToastType"] = "error";
                return View("~/Views/Admin/Servicios/ActualizarServicioForm.cshtml", actualizarServicioRequest);
            }

            string url = $"https://localhost:7210/api/Servicio/actualizar";

            try
            {
                var jsonContent = JsonConvert.SerializeObject(actualizarServicioRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["ToastMessage"] = "Servicio actualizado correctamente.";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("AdminServicios", "Admin");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["ToastMessage"] = $"No se pudo actualizar el servicio. Detalles: {errorContent}";
                    TempData["ToastType"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Ocurrió un error al intentar actualizar el servicio: {ex.Message}";
                TempData["ToastType"] = "error";
            }

            return View("~/Views/Admin/Servicios/ActualizarServicioForm.cshtml", actualizarServicioRequest);
        }


    }
}
