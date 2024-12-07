using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion_proyecto2.Models;
using System.Runtime.Intrinsics.Arm;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace aplicacion_proyecto2.Controllers
{
    public class TblPedidoesController : Controller
    {
        private readonly db_carritoContext _context;

        public IConfiguration Configuration { get; }

        public TblPedidoesController(db_carritoContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: TblPedidoes
        public async Task<IActionResult> Index(int id)
        {
            var db_carritoContext = _context.TblPedidos.Include(t => t.IdUsuarioNavigation).Where(p => p.IdUsuario == id && p.Estado == "A");
            return View(await db_carritoContext.ToListAsync());
            
        }

        public IActionResult Pedidos(int id, int idP)
        {
            TempData["Usuario"] = id;
            TempData["Header"] = "S";

            List<TblListaDetallePedido> pedido = new List<TblListaDetallePedido>();

            //Se carga la lista del pedido
            pedido = ListaPedidos(id, idP);

            if (pedido.Count() > 0)
            {
                ViewData["ExistenPedidos"] = "S";
                //Se muestra la lista del pedido en la vista
                ViewBag.ListaPedidos = pedido;
            }
            else
            {
                ViewData["ExistenPedidos"] = "N";
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Pedidos(int id, int idP, TblPedidoesController cr)
        {
            TempData["Usuario"] = id;
            TempData["Header"] = "S";
            return View();
        }

        //Función que devuelve una lista con el detalle del pedido
        public List<TblListaDetallePedido> ListaPedidos(int id, int idP)
        {
            //Se declaran objetos del tipo de modelo TblListaDetallePedido
            List<TblListaDetallePedido> detallePedido = new List<TblListaDetallePedido>();

            //Se obtienen todos los productos del pedido del cliente
            var query = "select t.id_detalle_pedido, t.id_pedido, t.id_detalle_producto, p.id_usuario, t.cantidad, d.nombre_producto, d.precio, c.color, m.medida from db_carrito.dbo.tbl_detalle_pedido t, db_carrito.dbo.tbl_pedidos p, db_carrito.dbo.tbl_productos d, db_carrito.dbo.tbl_detalle_producto e, db_carrito.dbo.tbl_colores c, db_carrito.dbo.tbl_medidas m where t.id_pedido = p.id_pedido and t.id_detalle_producto = e.id_detalle_producto and e.id_producto = d.id_producto and c.id_color = e.id_color and m.id_medida = e.id_medida and p.id_pedido = '" + idP + "' and p.id_usuario = '" + id + "';";

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
                                detallePedido.Add(new TblListaDetallePedido
                                {
                                    IdDetallePedido = read.GetInt32(0),
                                    IdPedido = read.GetInt32(1),
                                    IdDetalleProducto = read.GetInt32(2),                                   
                                    IdUsuario = read.GetInt32(3),
                                    Cantidad = read.GetInt32(4),
                                    NombreProducto = read.GetString(5),                                    
                                    Precio = read.GetDecimal(6),
                                    Color = read.GetString(7),
                                    Medida = read.GetString(8)
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

            return detallePedido;
        }

            // GET: TblPedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblPedidos == null)
            {
                return NotFound();
            }

            var tblPedido = await _context.TblPedidos
                .Include(t => t.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (tblPedido == null)
            {
                return NotFound();
            }

            return View(tblPedido);
        }

        // GET: TblPedidoes/Create
        public IActionResult Create(int idU, decimal idM)
        {
            ViewData["MontoCarrito"] = idM;
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario");
            TempData["Usuario"] = idU;
            return View();
        }

        // POST: TblPedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPedido,IdUsuario,MontoTotal,Telefono,Direccion,Fecha")] TblPedido tblPedido)
        {            
            DateTime fechaActual = DateTime.Parse(DateTime.Today.ToShortDateString());
            TblDetalleProducto detalle = new TblDetalleProducto();
            List<TblCarrito> listaCarrito = new List<TblCarrito>();
            TblProducto producto = new TblProducto();

            //--
            var query = "";
            var secuenciaIdPedido = 0;
            var secuenciaIdDetPedido = 0;
            var countPedido = _context.TblPedidos.Count();
            var countDetPedido = _context.TblDetallePedidos.Count();

            try
            {
                //Se realiza el autoincremento del pedido
                if (countPedido == 0)
                {
                    secuenciaIdPedido = 1;
                }
                else
                {
                    secuenciaIdPedido = _context.TblPedidos.Max(p => Convert.ToInt32(p.IdPedido));
                    secuenciaIdPedido += 1;
                }

                //Se realiza el autoincremento del detalle del pedido
                if (countDetPedido == 0)
                {
                    secuenciaIdDetPedido = 1;
                }
                else
                {
                    secuenciaIdDetPedido = _context.TblDetallePedidos.Max(p => Convert.ToInt32(p.IdDetallePedido));
                    secuenciaIdDetPedido += 1;
                }

                //Se inserta la información en la tabla de pedidos
                tblPedido.IdPedido = secuenciaIdPedido;
                tblPedido.Fecha = fechaActual;
                _context.Add(tblPedido);

                await _context.SaveChangesAsync();

                try
                {
                    //Se obtiene la información del carrito
                    listaCarrito = (from p in _context.TblCarritos
                               where p.IdUsuario == tblPedido.IdUsuario
                               select new TblCarrito
                               {
                                   IdCarrito = p.IdCarrito,
                                   IdUsuario = p.IdUsuario,
                                   IdDetalleProducto = p.IdDetalleProducto,
                                   Cantidad = p.Cantidad
                               }).ToList();

                    //Se recorre la lista                    
                    foreach(TblCarrito i in listaCarrito)
                    {
                        detalle = _context.TblDetalleProductos.FirstOrDefault(p => p.IdDetalleProducto == i.IdDetalleProducto);
                        //Se busca el precio del producto
                        producto = _context.TblProductos.FirstOrDefault(p => p.IdProducto == detalle.IdProducto);
                        
                        //Se calcula el monto del producto según la cantidad elegida
                        int? montoProducto = Convert.ToInt32(producto.Precio) * i.Cantidad;

                        //Se inserta la información en la tabla de detalle de pedidos
                        query = "insert into db_carrito.dbo.tbl_detalle_pedido(id_detalle_pedido,id_pedido,id_detalle_producto,cantidad,total_pedido) values('" + secuenciaIdDetPedido + "', '" + secuenciaIdPedido + "', '" + i.IdDetalleProducto + "', '" + i.Cantidad + "', '" + montoProducto + "');";


                        using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                        {
                            using (SqlCommand com = new SqlCommand(query, sqlConn))
                            {
                                sqlConn.Open();
                                com.ExecuteNonQuery();
                                sqlConn.Close();
                            }
                        }

                        montoProducto = 0;
                    }
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = ex.ToString();
                }                

                //Se elimina la información de la tabla carrito si se actualizó correctamente la de detalle de pedidos
                try
                {
                    var queryDelete = "delete db_carrito.dbo.tbl_carrito where id_usuario = '" + tblPedido.IdUsuario + "';";

                    using (SqlConnection sqlConn = new SqlConnection(Configuration["ConnectionStrings:conexion"]))
                    {
                        using (SqlCommand com = new SqlCommand(queryDelete, sqlConn))
                        {
                            sqlConn.Open();
                            com.ExecuteNonQuery();
                            sqlConn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    TempData["Mensaje"] = ex.ToString();
                }

                TempData["Usuario"] = tblPedido.IdUsuario;
                return RedirectToAction("Factura", "TblPedidoes", new { id = tblPedido.IdUsuario, idP = secuenciaIdPedido });
            }
            catch (Exception e) {
                TempData["Mensaje"] = "Ocurrió un error al intentar realizar la reserva" + e.ToString();
                ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblPedido.IdUsuario);
                TempData["Usuario"] = tblPedido.IdUsuario;
                return View(tblPedido);
            }
        }

        //Función que genera la factura
        public IActionResult Factura(int id, int idP)
        {
            TempData["Usuario"] = id;
            List<TblListaFactura> lista = new
                List<TblListaFactura>();

            lista = ListaFactura(id, idP);

            ViewBag.ListaFactura = lista;
            return View();
        }

        public List<TblListaFactura> ListaFactura(int id, int idP)
        {
            List<TblListaFactura> factura = new List<TblListaFactura>();

            TblListaFactura lista = new TblListaFactura();

            lista.pedido = _context.TblPedidos.FirstOrDefault(p => p.IdPedido == idP);

            lista.detalle = ListaPedidos(id, idP);

            lista.usuario = _context.TblUsuarios.FirstOrDefault(p => p.IdUsuario == id);

            lista.persona = _context.TblPersonas.FirstOrDefault(p => p.IdPersona == lista.usuario.IdPersona);

            factura.Add(lista);

            return factura;
        }

        // GET: TblPedidoes/Edit/5
        public IActionResult Edit(int? id, int idU)
        {
            TempData["Usuario"] = idU;
            ViewData["idReserva"] = id;
            return View();
        }

        // POST: TblPedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            TblPedido pedido = new TblPedido();
            pedido = _context.TblPedidos.FirstOrDefault(p => p.IdPedido == id);

            try
            {
                pedido.Estado = "I";

                _context.Update(pedido);
                await _context.SaveChangesAsync();

                TempData["Usuario"] = pedido.IdUsuario;

                return RedirectToAction("Index", "TblPedidoes", new { id = pedido.IdUsuario });
            }
            catch (Exception ex)
            {
                TempData["Usuario"] = pedido.IdUsuario;
                ViewData["idReserva"] = id;
                ViewData["Error"] = "Ha ocurrido un error cancelando el pedido. Error: " + ex;
                return View();
            }
        }

        // GET: TblPedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblPedidos == null)
            {
                return NotFound();
            }

            var tblPedido = await _context.TblPedidos
                .Include(t => t.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdPedido == id);
            if (tblPedido == null)
            {
                return NotFound();
            }

            return View(tblPedido);
        }

        // POST: TblPedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblPedidos == null)
            {
                return Problem("Entity set 'db_carritoContext.TblPedidos'  is null.");
            }
            var tblPedido = await _context.TblPedidos.FindAsync(id);
            if (tblPedido != null)
            {
                _context.TblPedidos.Remove(tblPedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblPedidoExists(int id)
        {
          return (_context.TblPedidos?.Any(e => e.IdPedido == id)).GetValueOrDefault();
        }
    }
}
