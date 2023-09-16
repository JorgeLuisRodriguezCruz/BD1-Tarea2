using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Tarea__2.Data;
using Tarea__2.Models;

namespace Tarea__2.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _context;

        //public HomeController(ILogger<HomeController> logger)
        public HomeController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ArticuloEntity> articulos = (IEnumerable<ArticuloEntity>)_context.Articulo.FromSqlInterpolated($"SP_ConsultaOrdenadaAlfabticamente").AsAsyncEnumerable();
            IEnumerable<ClaseArticuloEntity> clases = _context.ClaseArticulo.ToList();

            List <ArticuloVista> listaArticulos = new List<ArticuloVista>();

            foreach (var articulo in articulos)
            {
                foreach (var clase in clases)
                {
                    if (clase.Id == articulo.IdClaseArticulo) {
                        ArticuloVista artVista = new ArticuloVista(clase.Nombre, articulo.Codigo, articulo.Nombre, articulo.Precio);

                        listaArticulos.Add(artVista);
                    }
                    
                }
            }
            IEnumerable<ArticuloVista> articulosVista = listaArticulos;

            return View(articulosVista);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Start()
        {
            return View();
        }

        public IActionResult Insertar() { 
            return View(); 
        }

        public IActionResult Modificar() { 
            return View(); 
        }

        public async Task<IActionResult> Borrar(int? id) {

            if (id == null || _context.Articulo == null)
            {
                return NotFound();
            }

            var entidadArticulo = await _context.Articulo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entidadArticulo == null)
            {
                return NotFound();
            }

            return View(entidadArticulo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrarConfirmado(int id)
        {
            if (_context.Articulo == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Articulo'  is null.");
            }
            var entidadArticulo = await _context.Articulo.FindAsync(id);
            if (entidadArticulo != null)
            {
                _context.Articulo.Remove(entidadArticulo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Salir() { 
            return RedirectToAction("Login", "Acceso"); 
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}