using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using Tarea__2.Data;
using Tarea__2.Models;

namespace Tarea__2.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ApplicationDBContext _context;

        //public HomeController(ILogger<HomeController> logger)
        public HomeController(ApplicationDBContext context)
        {
            _context = context;
        }

        private IEnumerable<ArticuloEntity> obtenerArticulos()
        {
            return (IEnumerable<ArticuloEntity>)_context.Articulo.FromSqlInterpolated($"SP_ConsultaOrdenadaAlfabticamente").AsAsyncEnumerable();
        }

        private List<ClaseArticuloEntity> obtenerClasesArt()
        {
            return _context.ClaseArticulo.ToList();
        }

        private List<ArticuloVista> obtenerTodosArticulos(IEnumerable<ArticuloEntity> articulos, List<ClaseArticuloEntity> clases)
        {
            List<ArticuloVista> listaArticulos = new List<ArticuloVista>();
            foreach (var articulo in articulos)
            {
                foreach (var clase in clases)
                {
                    if (clase.Id == articulo.IdClaseArticulo)
                    {
                        ArticuloVista artVista = new ArticuloVista(clase.Nombre, articulo.Codigo, articulo.Nombre, articulo.Precio);

                        listaArticulos.Add(artVista);
                    }

                }
            }
            return listaArticulos;
        }

        private List<ArticuloVista> obtenerArticulosPorClase(IEnumerable<ArticuloEntity> articulos, List<ClaseArticuloEntity> clases, int claseCondicional)
        {
            List<ArticuloVista> listaArticulos = new List<ArticuloVista>();
            foreach (var articulo in articulos)
            {
                foreach (var clase in clases)
                {
                    if (clase.Id == articulo.IdClaseArticulo && claseCondicional == articulo.IdClaseArticulo)
                    {
                        ArticuloVista artVista = new ArticuloVista(clase.Nombre, articulo.Codigo, articulo.Nombre, articulo.Precio); 
                        listaArticulos.Add(artVista);
                    }
                }
            }
            return listaArticulos;
        }

        private List<SelectListItem> obtenerItemsComboBox (List<ClaseArticuloEntity> clases)
        {
            return clases.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.Id.ToString(),
                    Selected = false
                };
            });
        }

        private ArticuloVista obtenerArticuloVista(ArticuloEntity articulo)
        {
            List<ClaseArticuloEntity> clases = this.obtenerClasesArt();
            ArticuloVista artVista = null;
            foreach (var clase in clases)
            {
                if (clase.Id == articulo.IdClaseArticulo)
                {
                    artVista = new ArticuloVista(clase.Nombre, articulo.Codigo, articulo.Nombre, articulo.Precio);
                }
            }
            return artVista;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<ArticuloEntity> articulos = this.obtenerArticulos();
            List<ClaseArticuloEntity> clases = this.obtenerClasesArt();
            IEnumerable<ArticuloVista> articulosVista = this.obtenerTodosArticulos(articulos, clases);

            List<SelectListItem> items = this.obtenerItemsComboBox(clases);

            ViewBag.opciones = items;

            return View(articulosVista);
        }

        [HttpPost]
        //public ActionResult Index(IEnumerable<ArticuloVista> articulosVista, List<ClaseArticuloEntity> clases) 
        public ActionResult Index(string NombreFiltro, string CantidadFiltro, string claseArt)
        {

            IEnumerable<ArticuloEntity> articulos = this.obtenerArticulos();
            List<ClaseArticuloEntity> clases = this.obtenerClasesArt();
            IEnumerable<ArticuloVista> articulosVista;
            List<SelectListItem> items= new List<SelectListItem>();

            if (claseArt != null)
            {
                int idCondicional = int.Parse(claseArt);
                articulosVista = this.obtenerArticulosPorClase(articulos, clases, idCondicional);

                items = this.obtenerItemsComboBox(clases);
                ViewBag.opciones = items;

                return View(articulosVista);
            }

            articulosVista = this.obtenerTodosArticulos(articulos, clases);
            items = this.obtenerItemsComboBox(clases);
            ViewBag.opciones = items;

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

        public IActionResult Insertar() 
        { 
            List<ClaseArticuloEntity> clases = this.obtenerClasesArt();
            List<SelectListItem> items = this.obtenerItemsComboBox(clases);
            ViewBag.opciones = items;
            return View(); 
        }

        public async Task<IActionResult> Modificar()
        {
            string? codigo = TempData["Codigo"] as string;
            if (codigo != null && _context != null)
            {
                ArticuloEntity articulo = _context.Articulo.FirstOrDefault(e => e.Codigo == codigo);
                if (articulo != null)
                {
                    ArticuloVista articuloBuscado = this.obtenerArticuloVista(articulo);

                    List<ClaseArticuloEntity> clases = this.obtenerClasesArt(); 
                    List<SelectListItem> items = this.obtenerItemsComboBox(clases); 
                    ViewBag.opciones = items;


                    return View(articuloBuscado);
                }
            }
            return NotFound();
        }

        public IActionResult Select()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SelectRedoreccion(string Codigo, string Accion)
        {
            
            if (Codigo != null && Codigo != "")
            {
                try
                {
                    ArticuloEntity articulo = _context.Articulo.FirstOrDefault(e => e.Codigo == Codigo);

                    if (articulo != null)
                    {

                        if (Accion.ToUpper().Contains("MODIFICAR") == true)
                        {
                            TempData["Codigo"] = Codigo;
                            return RedirectToAction(nameof(Modificar));
                        }
                        if (Accion.ToUpper().Contains("BORRAR") == true)
                        {
                            TempData["Codigo"] = Codigo;
                            return RedirectToAction(nameof(Borrar));
                        }
                    }

                } catch (Exception ex)
                { 
                    ViewBag.Subtitulo = Accion;
                    ViewBag.Mensaje = "Error: --> " + ex.Message; ;

                    return View(nameof(Select));
                } 
            } 
            ViewBag.Subtitulo = Accion;
            ViewBag.Mensaje = "Error: El articulo no ha sido encontrado.";
            return View(nameof(Select));
        }

        public async Task<IActionResult> Borrar()
        {
            string? codigo = TempData["Codigo"] as string;
            if (codigo != null && _context != null) 
            {
                ArticuloEntity articulo = _context.Articulo.FirstOrDefault(e => e.Codigo == codigo);
                if (articulo != null)
                {
                    ArticuloVista articuloBuscado = this.obtenerArticuloVista(articulo);

                    return View(articuloBuscado);
                }
            } 
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrarConfirmado(string Codigo)
        {
            /*
            if (_context.Articulo == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Articulo'  is null.");
            }
            var entidadArticulo = await _context.Articulo.FindAsync(id);
            if (entidadArticulo != null)
            {
                _context.Articulo.Remove(entidadArticulo);
            }

            await _context.SaveChangesAsync();*/
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
    
    

        [HttpPost]
        public ActionResult FiltrarClaseARticulo (string claseArt)
        {
            IEnumerable<ArticuloEntity> articulos = this.obtenerArticulos();
            List<ClaseArticuloEntity> clases = this.obtenerClasesArt();
            IEnumerable<ArticuloVista> articulosVista;
             
            if (claseArt != null)
            {
                int idCondicional = int.Parse(claseArt);
                articulosVista = this.obtenerArticulosPorClase(articulos, clases, idCondicional);
                return RedirectToAction(nameof(Index));
            } else 
            {
                return RedirectToAction(nameof(Index)); 
            }
        }


        public IActionResult BanderaModificar()
        {  
            ViewBag.Subtitulo = "Modificar articulo";
            return View(nameof(Select));
        }

        public IActionResult BanderaBorrar()
        { 
            ViewBag.Subtitulo = "Borrar articulo";
            return View(nameof(Select));
        }


    }
}