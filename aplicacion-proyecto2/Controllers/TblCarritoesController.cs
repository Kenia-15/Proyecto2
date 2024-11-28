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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                ViewData["CantArticulos"] = 0;
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
        public IActionResult Create(string nom, int stk, int idD, int idC)
        {
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto");
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario");

            ViewData["NombreProducto"] = nom;
            ViewData["Stock"] = stk;
            ViewData["IdDetalle"] = idD;
            ViewData["Categoria"] = idC;

            return View();
        }

        // POST: TblCarritoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCarrito,IdUsuario,IdDetalleProducto,Cantidad")] TblCarrito tblCarrito)
        {

            TblDetalleProducto enStock = new TblDetalleProducto();
            TblCarrito carrito = new TblCarrito();
            int? cantidadNueva = 0;
            int? nuevoStock = 0;

            //Se obtiene la cantidad de productos en stock
            enStock = _context.TblDetalleProductos.FirstOrDefault(p => p.IdDetalleProducto.Equals(tblCarrito.IdDetalleProducto));

            //Se valida si la cantidad de productos seleccionada es congruente con la cantidad de productos en stock
            if (enStock.Stock < tblCarrito.Cantidad)
            {
                ViewData["Mensaje"] = "La cantidad del producto no puede ser mayor a la cantidad de productos en stock";
            }
            else
            {
                try
                {
                    //Se obtiene la información del carrito
                    carrito = _context.TblCarritos.FirstOrDefault(p => p.IdDetalleProducto.Equals(tblCarrito.IdDetalleProducto));

                    //Si el producto ya existe en el carrito, se procede a actualizar la cantidad del producto
                    if (carrito is not null)
                    {
                        if (carrito.Cantidad > 0)
                        {
                            cantidadNueva = carrito.Cantidad + tblCarrito.Cantidad;

                            var queryUpdate = "update db_carrito.dbo.tbl_carrito set cantidad = '" + cantidadNueva + "' where id_detalle_producto = '" + tblCarrito.IdDetalleProducto + "';";

                            try
                            {
                                using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                                {
                                    using (SqlCommand com = new SqlCommand(queryUpdate, sqlConn))
                                    {
                                        sqlConn.Open();
                                        com.ExecuteNonQuery();
                                        sqlConn.Close();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewData["Mensaje"] = "Ocurrió un error al intentar actualizar el producto" + ex.ToString();
                            }
                        }
                    }
                    else
                    {
                        //De lo contrario se agrega el producto al carrito
                        _context.Add(tblCarrito);
                        await _context.SaveChangesAsync();
                    }

                    //Se procede a actualizar el stock del producto agregado al carrito
                    nuevoStock = enStock.Stock - tblCarrito.Cantidad;

                    try
                    {
                        var queryUpd = "update db_carrito.dbo.tbl_detalle_producto set stock = '" + nuevoStock + "' where id_detalle_producto = '" + tblCarrito.IdDetalleProducto + "';";

                        using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                        {
                            using (SqlCommand com = new SqlCommand(queryUpd, sqlConn))
                            {
                                sqlConn.Open();
                                com.ExecuteNonQuery();
                                sqlConn.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewData["Mensaje"] = "Ocurrió un error al intentar actualizar el stock producto" + ex.ToString();
                    }


                    return RedirectToAction("Carrito", "TblCarritoes", new { id = tblCarrito.IdUsuario });
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = "Ocurrió un error al intentar agregar el producto al carrito " + ex.ToString();
                }
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
        public async Task<IActionResult> Edit(int id, int idU, [Bind("IdCarrito,IdUsuario,IdDetalleProducto,Cantidad")] TblCarrito tblCarrito)
        {
            TblDetalleProducto enStock = new TblDetalleProducto();
            int? cantidadProducto = 0;
            int? nuevoStock = 0;
            int? diferencia = 0;

            //Se obtiene la información del carrito
            try
            {
                var query = "select cantidad from db_carrito.dbo.tbl_carrito where id_carrito = '" + id + "';";

                using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                {
                    using (SqlCommand com = new SqlCommand(query, sqlConn))
                    {
                        sqlConn.Open();

                        using (SqlDataReader read = com.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                cantidadProducto = read.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex;
            }
            //carrito = _context.TblCarritos.FirstOrDefault(p => p.IdCarrito.Equals(tblCarrito.IdCarrito));

            //Se obtiene la cantidad de productos en stock
            enStock = _context.TblDetalleProductos.FirstOrDefault(p => p.IdDetalleProducto.Equals(tblCarrito.IdDetalleProducto));

            //Si la cantidad de productos aumentó
            if (tblCarrito.Cantidad > cantidadProducto)
            {
                //Se calcula la diferencia de la cantidad de productos
                diferencia = tblCarrito.Cantidad - cantidadProducto;

                nuevoStock = enStock.Stock - diferencia;
            } //Si la cantidad de productos redujo
            else if (tblCarrito.Cantidad < cantidadProducto)
            {
                //Se calcula la diferencia de la cantidad de productos
                diferencia = cantidadProducto - tblCarrito.Cantidad;

                nuevoStock = enStock.Stock + diferencia;
            }

            if (id != tblCarrito.IdCarrito)
            {
                return NotFound();
            }

            try
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

                try
                {
                    var queryUpd = "update db_carrito.dbo.tbl_detalle_producto set stock = '" + nuevoStock + "' where id_detalle_producto = '" + tblCarrito.IdDetalleProducto + "';";

                    using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                    {
                        using (SqlCommand com = new SqlCommand(queryUpd, sqlConn))
                        {
                            sqlConn.Open();
                            com.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewData["Mensaje"] = "Ocurrió un error al intentar editar el producto" + ex.ToString();
                }

                return RedirectToAction("Carrito", "TblCarritoes", new { id = tblCarrito.IdUsuario });
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Ocurrió un error al intentar editar el producto" + ex.ToString();
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
            TblDetalleProducto enStock = new TblDetalleProducto();
            TblCarrito carrito = new TblCarrito();
            ViewData["IdUsuario"] = idU;
            int? nuevoStock = 0;

            //Se obtiene la información del carrito
            carrito = _context.TblCarritos.FirstOrDefault(p => p.IdCarrito.Equals(id));

            //Se obtiene la cantidad de productos en stock
            enStock = _context.TblDetalleProductos.FirstOrDefault(p => p.IdDetalleProducto.Equals(carrito.IdDetalleProducto));

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

            //Se procede a actualizar el stock del producto eliminado del carrito
            nuevoStock = enStock.Stock + carrito.Cantidad;

            try
            {
                var queryUpd = "update db_carrito.dbo.tbl_detalle_producto set stock = '" + nuevoStock + "' where id_detalle_producto = '" + carrito.IdDetalleProducto + "';";

                using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                {
                    using (SqlCommand com = new SqlCommand(queryUpd, sqlConn))
                    {
                        sqlConn.Open();
                        com.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData["Mensaje"] = "Ocurrió un error al intentar actualizar el stock producto" + ex.ToString();
            }

            idU = carrito.IdUsuario;
            return RedirectToAction("Carrito", "TblCarritoes", new { id = idU });
        }

        private bool TblCarritoExists(int id)
        {
          return (_context.TblCarritos?.Any(e => e.IdCarrito == id)).GetValueOrDefault();
        }
    }
}
