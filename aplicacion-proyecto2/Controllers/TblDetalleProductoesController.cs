using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion_proyecto2.Models;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Threading;

namespace aplicacion_proyecto2.Controllers
{
    public class TblDetalleProductoesController : Controller
    {
        private readonly db_carritoContext _context;

        public TblDetalleProductoesController(db_carritoContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET: TblDetalleProductoes
        public async Task<IActionResult> Index()
        {
            var db_carritoContext = _context.TblDetalleProductos.Include(t => t.IdColorNavigation).Include(t => t.IdMedidaNavigation).Include(t => t.IdProductoNavigation);
            return View(await db_carritoContext.ToListAsync());
        }

        //Procedimiento encargado de mostrar los productos por categoría
        [Route("api/[controller]")]
        public IActionResult Productos(string id, int idC, string buscar)
        {
            TempData["Categoria"] = idC;

            List<TblListaProducto> productos = new List<TblListaProducto>();

            //Se carga la lista de productos
            productos = ListaProductos(idC);

            //Se filtra la lista de productos según el valor que trae el parámetro "buscar" 
            if (!String.IsNullOrEmpty(buscar))
            {
                //Se busca en la lista de productos el producto independientemente de que sea en mayúscula o minúscula
                productos = productos.Where(p => p.NombreProducto.ToLower().Contains(buscar.ToLower()) || p.NombreProducto.ToUpper().Contains(buscar.ToUpper())).ToList();
            }

            //Se muestra la lista de productos en la vista
            ViewBag.Productos = productos;
            return View();
        }

        [HttpPost]
        public IActionResult Productos(TblDetalleProductoesController det, int id, string idC)
        {
            TempData["Categoria"] = idC;
            return View();
        }

        //Función que devuelve una lista con los productos por categoría
        public List<TblListaProducto> ListaProductos(int idCategoria)
        {
            //Se declaran objetos del tipo de modelo TblDetalleProducto
            List<TblListaProducto> detProducto = new List<TblListaProducto>();
            TblListaProducto det = new TblListaProducto();

            //Se obtienen todos los productos de la categoría ingresada por parámetro
            var query = "select t.id_detalle_producto, t.stock, p.id_producto, p.nombre_producto, p.descripcion, p.precio, c.color, m.medida, t.ruta_imagen from db_carrito.dbo.tbl_productos p, db_carrito.dbo.tbl_detalle_producto t, db_carrito.dbo.tbl_colores c, db_carrito.dbo.tbl_medidas m where t.id_producto = p.id_producto and c.id_color = t.id_color and m.id_medida = t.id_medida and p.id_categoria = '"+ idCategoria + "' and t.stock > 0;";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                {
                    using (SqlCommand com = new SqlCommand(query, sqlConn))
                    {
                        sqlConn.Open();

                        using (SqlDataReader read = com.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                detProducto.Add(new TblListaProducto
                                {
                                    IdDetalleProducto = read.GetInt32(0),
                                    Stock = read.GetInt32(1),
                                    IdProducto = read.GetInt32(2),
                                    NombreProducto = read.GetString(3),
                                    Descripcion = read.GetString(4),
                                    Precio = read.GetDecimal(5),
                                    Color = read.GetString(6),
                                    Medida = read.GetString(7),
                                    RutaImagen = read.GetString(8)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            
            return detProducto;
        }

        //funci{on que devuelve la lista del detalle de los productos
		public List<TblListaProducto> DetalleProducto(int idDetalle)
		{
			//Se declaran objetos del tipo de modelo TblDetalleProducto
			List<TblListaProducto> detProducto = new List<TblListaProducto>();
			TblListaProducto det = new TblListaProducto();

			//Se obtienen todos los productos de la categoría ingresada por parámetro
			var query = "select t.id_detalle_producto, t.stock, p.id_producto, p.nombre_producto, p.descripcion, p.precio, c.color, m.medida, t.ruta_imagen from db_carrito.dbo.tbl_productos p, db_carrito.dbo.tbl_detalle_producto t, db_carrito.dbo.tbl_colores c, db_carrito.dbo.tbl_medidas m where t.id_producto = p.id_producto and c.id_color = t.id_color and m.id_medida = t.id_medida and t.id_detalle_producto = '" + idDetalle + "' and t.stock > 0;";

			try
			{
				using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
				{
					using (SqlCommand com = new SqlCommand(query, sqlConn))
					{
						sqlConn.Open();

						using (SqlDataReader read = com.ExecuteReader())
						{
							while (read.Read())
							{
								detProducto.Add(new TblListaProducto
								{
									IdDetalleProducto = read.GetInt32(0),
									Stock = read.GetInt32(1),
									IdProducto = read.GetInt32(2),
									NombreProducto = read.GetString(3),
									Descripcion = read.GetString(4),
									Precio = read.GetDecimal(5),
									Color = read.GetString(6),
									Medida = read.GetString(7),
									RutaImagen = read.GetString(8)
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());
			}

			return detProducto;
		}


		// GET: TblDetalleProductoes/Details/5
		public IActionResult Details(int idD, int idC)
        {
            TempData["Categoria"] = idC;

            List<TblListaProducto> productos = new List<TblListaProducto>();

            //Se carga la lista de productos
            productos = DetalleProducto(idD);

            if (idD == 0 || _context.TblDetalleProductos == null)
            {
                return NotFound();
            }

            //Se muestra la lista de productos en la vista
            ViewBag.Productos = productos;
            return View();
        }

        // GET: TblDetalleProductoes/Create
        public IActionResult Create()
        {
            ViewData["IdColor"] = new SelectList(_context.TblColores, "IdColor", "IdColor");
            ViewData["IdMedida"] = new SelectList(_context.TblMedidas, "IdMedida", "IdMedida");
            ViewData["IdProducto"] = new SelectList(_context.TblProductos, "IdProducto", "IdProducto");
            return View();
        }

        // POST: TblDetalleProductoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalleProducto,IdProducto,IdMedida,IdColor,Stock,NombreImagen,RutaImagen")] TblDetalleProducto tblDetalleProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblDetalleProducto);
                await _context.SaveChangesAsync();

                //Ver como se redireccionaba
                //return RedirectToAction(nameof(Carrito));
            }
            ViewData["IdColor"] = new SelectList(_context.TblColores, "IdColor", "IdColor", tblDetalleProducto.IdColor);
            ViewData["IdMedida"] = new SelectList(_context.TblMedidas, "IdMedida", "IdMedida", tblDetalleProducto.IdMedida);
            ViewData["IdProducto"] = new SelectList(_context.TblProductos, "IdProducto", "IdProducto", tblDetalleProducto.IdProducto);
            return View(tblDetalleProducto);
        }

        // GET: TblDetalleProductoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblDetalleProductos == null)
            {
                return NotFound();
            }

            var tblDetalleProducto = await _context.TblDetalleProductos.FindAsync(id);
            if (tblDetalleProducto == null)
            {
                return NotFound();
            }
            ViewData["IdColor"] = new SelectList(_context.TblColores, "IdColor", "IdColor", tblDetalleProducto.IdColor);
            ViewData["IdMedida"] = new SelectList(_context.TblMedidas, "IdMedida", "IdMedida", tblDetalleProducto.IdMedida);
            ViewData["IdProducto"] = new SelectList(_context.TblProductos, "IdProducto", "IdProducto", tblDetalleProducto.IdProducto);
            return View(tblDetalleProducto);
        }

        // POST: TblDetalleProductoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalleProducto,IdProducto,IdMedida,IdColor,Stock,NombreImagen,RutaImagen")] TblDetalleProducto tblDetalleProducto)
        {
            if (id != tblDetalleProducto.IdDetalleProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblDetalleProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblDetalleProductoExists(tblDetalleProducto.IdDetalleProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdColor"] = new SelectList(_context.TblColores, "IdColor", "IdColor", tblDetalleProducto.IdColor);
            ViewData["IdMedida"] = new SelectList(_context.TblMedidas, "IdMedida", "IdMedida", tblDetalleProducto.IdMedida);
            ViewData["IdProducto"] = new SelectList(_context.TblProductos, "IdProducto", "IdProducto", tblDetalleProducto.IdProducto);
            return View(tblDetalleProducto);
        }

        // GET: TblDetalleProductoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblDetalleProductos == null)
            {
                return NotFound();
            }

            var tblDetalleProducto = await _context.TblDetalleProductos
                .Include(t => t.IdColorNavigation)
                .Include(t => t.IdMedidaNavigation)
                .Include(t => t.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetalleProducto == id);
            if (tblDetalleProducto == null)
            {
                return NotFound();
            }

            return View(tblDetalleProducto);
        }

        // POST: TblDetalleProductoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idU)
        {

            if (_context.TblDetalleProductos == null)
            {
                return Problem("Entity set 'db_carritoContext.TblDetalleProductos'  is null.");
            }
            var tblDetalleProducto = await _context.TblDetalleProductos.FindAsync(id);
            if (tblDetalleProducto != null)
            {
                _context.TblDetalleProductos.Remove(tblDetalleProducto);
            }

            ViewData["IdProducto"] = idU;

            await _context.SaveChangesAsync();
            return RedirectToAction("Carrito","TblCarritoes", new { id = idU} );
        }

        private bool TblDetalleProductoExists(int id)
        {
          return (_context.TblDetalleProductos?.Any(e => e.IdDetalleProducto == id)).GetValueOrDefault();
        }
    }
}
