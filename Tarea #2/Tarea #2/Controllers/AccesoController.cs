using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tarea__2.Data;
using Tarea__2.Models;
using System;
using System.IO;

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
            //string rut = "C:\\Users\\rodri\\Desktop\\GIT\\BD1-Tarea2\\Tarea #2\\Tarea #2\\DatosBase\\DatosDe@daTarea232.xml";
            //string xmlContent="";
            
            try
            {
                //xmlContent = System.IO.File.ReadAllText(rut);
                //Console.WriteLine(xmlContent);

                SqlConnection conn = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand cmd = conn.CreateCommand();
                conn.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.CommandText = "CargaXML";
                //cmd.CommandText = "carggXML"; 
                cmd.CommandText = "ChargeXMLdata";
                //cmd.Parameters.Add("@xmlData", System.Data.SqlDbType.NVarChar, 500).Value = xmlContent; 
                cmd.ExecuteNonQuery();
                conn.Close(); 
                
            }
            catch (SqlException ex)
            { 
                string errorMessage = ex.Message;  
                Console.WriteLine("ERROR --> "+ errorMessage);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("El archivo XML no se encontró en la ruta especificada.");
            }

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
