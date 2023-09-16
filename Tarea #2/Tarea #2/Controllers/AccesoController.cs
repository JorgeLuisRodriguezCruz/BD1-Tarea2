using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tarea__2.Data;
using Tarea__2.Models;

namespace Tarea__2.Controllers
{
    public class AccesoController : Controller
    {

        private readonly ApplicationDBContext _context;

        public AccesoController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Nombre, string Contraseña)
        {
            //UserEntity usuario;
            if (Nombre != "")
            {
                try
                {
                    UserEntity usuario = (UserEntity)_context.Usuario.FirstOrDefault (e => e.Nombre == Nombre);

                    if (usuario != null && usuario.Contraseña.Equals(Contraseña))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                } catch (Exception ex)
                {
                    ViewBag.Mensaje = "ERROR ##" + ex.ToString();
                    return View();
                }
            }
            ViewBag.Mensaje = "Error: Combinación de usuario y password no existe en la BD.";
            return View();
        }

    }
}
