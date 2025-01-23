using Microsoft.AspNetCore.Mvc;

namespace WebApplicationMVC.Controllers
{
    
    public class AdminController : Controller
    {
        public IActionResult AdminHome(int id, string nombre)
        {
            ViewData["Id"] = id;
            ViewData["Nombre"] = nombre;
            return View();
        }
    }
}
