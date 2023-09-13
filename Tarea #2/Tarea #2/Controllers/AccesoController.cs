using Microsoft.AspNetCore.Mvc;

namespace Tarea__2.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Nombre, string Contraseña)
        {
            Console.WriteLine("Este es el nombre ingresado:  ---- > " + Nombre);
            return RedirectToAction("Index", "Home");
        }

    }
}
