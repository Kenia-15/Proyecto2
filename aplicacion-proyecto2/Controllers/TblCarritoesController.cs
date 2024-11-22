using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion_proyecto2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Routing;

namespace aplicacion_proyecto2.Controllers
{
    public class TblCarritoesController : Controller
    {
        private readonly db_carritoContext _context;

        public TblCarritoesController(db_carritoContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // GET: TblCarritoes
        public IActionResult Carrito(int id)
        {
            /*var db_carritoContext = _context.TblCarritos.Include(t => t.IdDetalleProductoNavigation).Include(t => t.IdUsuarioNavigation);
            return View(await db_carritoContext.ToListAsync());*/

            TempData["idUsuario"] = id;

            List<TblListaDetalleCarrito> carrito = new List<TblListaDetalleCarrito>();
            decimal montoTotal = 0;
            decimal montoCantProducto = 0;

            //Se carga la lista de carrito
            carrito = ListaCarrito(id);

            if (carrito.Count() > 0)
            {
                ViewData["ExistenProductos"] = "S";
                ViewData["CantArticulos"] = carrito.Count();

                foreach (var item in carrito)
                {
                    montoCantProducto = item.Cantidad * item.Precio;
                    montoTotal = montoTotal + montoCantProducto;
                }

                ViewData["MontoCarrito"] = montoTotal;

               //Se muestra la lista de carrito en la vista
               ViewBag.CarritoLista = carrito;
            } else
            {
                ViewData["ExistenProductos"] = "N";
            }            
            return View();
        }

        [HttpPost]
        public IActionResult Carrito(int id, TblCarritoesController cr)
        {
            TempData["idUsuario"] = id;
            return View();
        }


        // GET: TblCarritoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblCarritos == null)
            {
                return NotFound();
            }

            var tblCarrito = await _context.TblCarritos
                .Include(t => t.IdDetalleProductoNavigation)
                .Include(t => t.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCarrito == id);
            if (tblCarrito == null)
            {
                return NotFound();
            }

            return View(tblCarrito);
        }

        // GET: TblCarritoes/Create
        public IActionResult Create()
        {
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto");
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: TblCarritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarrito,IdUsuario,IdDetalleProducto,Cantidad")] TblCarrito tblCarrito)
        {
            //Se obtiene la cantidad de productos en stock

            //Se valida si la cantidad de productos seleccionada es congruente con la cantidad de productos en stock
            
            try
            {
                //Si el producto ya existe en el carrito, se procede a actualizar la cantidad del producto

                //De lo contrario se agrega el producto al carrito
                _context.Add(tblCarrito);
                await _context.SaveChangesAsync();

                //Se procede a actualizar el stock del producto agregado al carrito


                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Ocurrió un error al intentar agregar el producto al carrito " + ex.ToString();
            }

            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto", tblCarrito.IdDetalleProducto);
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblCarrito.IdUsuario);
            return View(tblCarrito);
        }

        //Función que devuelve una lista con el detalle del carrito
        public List<TblListaDetalleCarrito> ListaCarrito(int id)
        {
            //Se declaran objetos del tipo de modelo TblListaDetalleCarrito
            List<TblListaDetalleCarrito> detalleCarrito = new List<TblListaDetalleCarrito>();
            TblListaDetalleCarrito det = new TblListaDetalleCarrito();

            //Se obtienen todos los productos del carrito del cliente
            var query = "select t.id_detalle_producto, t.stock, p.id_producto, p.nombre_producto, p.descripcion, p.precio, c.color, m.medida, t.ruta_imagen, cr.id_carrito, cr.cantidad from db_carrito.dbo.tbl_productos p, db_carrito.dbo.tbl_detalle_producto t, db_carrito.dbo.tbl_colores c, db_carrito.dbo.tbl_medidas m, db_carrito.dbo.tbl_carrito cr where t.id_producto = p.id_producto and c.id_color = t.id_color and m.id_medida = t.id_medida and cr.id_detalle_producto = t.id_detalle_producto and cr.id_usuario = '" + id + "' ;";

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
                                detalleCarrito.Add(new TblListaDetalleCarrito
                                {
                                    IdDetalleProducto = read.GetInt32(0),
                                    Stock = read.GetInt32(1),
                                    IdProducto = read.GetInt32(2),
                                    NombreProducto = read.GetString(3),
                                    Descripcion = read.GetString(4),
                                    Precio = read.GetDecimal(5),
                                    Color = read.GetString(6),
                                    Medida = read.GetString(7),
                                    RutaImagen = read.GetString(8),
                                    IdCarrito = read.GetInt32(9),
                                    Cantidad = read.GetInt32(10)
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

            //Se calcula el total del carrito
            return detalleCarrito;
        }

        // GET: TblCarritoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblCarritos == null)
            {
                return NotFound();
            }

            var tblCarrito = await _context.TblCarritos.FindAsync(id);
            if (tblCarrito == null)
            {
                return NotFound();
            }
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto", tblCarrito.IdDetalleProducto);
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblCarrito.IdUsuario);
            return View(tblCarrito);
        }

        // POST: TblCarritoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCarrito,IdUsuario,IdDetalleProducto,Cantidad")] TblCarrito tblCarrito)
        {
            if (id != tblCarrito.IdCarrito)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCarrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCarritoExists(tblCarrito.IdCarrito))
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
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto", tblCarrito.IdDetalleProducto);
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblCarrito.IdUsuario);
            return View(tblCarrito);
        }

        // GET: TblCarritoes/Delete/5
        public async Task<IActionResult> Delete(int? id, int idU)
        {
            ViewData["IdUsuario"] = idU;

            if (id == null || _context.TblCarritos == null)
            {
                return NotFound();
            }

            var tblCarrito = await _context.TblCarritos
                .Include(t => t.IdDetalleProductoNavigation)
                .Include(t => t.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdCarrito == id);
            if (tblCarrito == null)
            {
                return NotFound();
            }

            return View(tblCarrito);
        }

        // POST: TblCarritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idU)
        {
            ViewData["IdUsuario"] = idU;

            if (_context.TblCarritos == null)
            {
                return Problem("Entity set 'db_carritoContext.TblCarritos'  is null.");
            }
            var tblCarrito = await _context.TblCarritos.FindAsync(id);
            if (tblCarrito != null)
            {
                _context.TblCarritos.Remove(tblCarrito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Carrito));

            //Revisar en el proyecto 1 cómo pase parámetros con el redirect
        }

        private bool TblCarritoExists(int id)
        {
          return (_context.TblCarritos?.Any(e => e.IdCarrito == id)).GetValueOrDefault();
        }
    }
}
