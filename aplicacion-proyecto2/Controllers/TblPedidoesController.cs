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
        public async Task<IActionResult> Index()
        {
            var db_carritoContext = _context.TblPedidos.Include(t => t.IdUsuarioNavigation);
            return View(await db_carritoContext.ToListAsync());
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
                return RedirectToAction("Carrito", "TblCarritoes", new { idU = tblPedido.IdUsuario });
            }
            catch (Exception e) {
                TempData["Mensaje"] = "Ocurrió un error al intentar realizar la reserva" + e.ToString();
                ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblPedido.IdUsuario);
                TempData["Usuario"] = tblPedido.IdUsuario;
                return View(tblPedido);
            }
        }

        // GET: TblPedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblPedidos == null)
            {
                return NotFound();
            }

            var tblPedido = await _context.TblPedidos.FindAsync(id);
            if (tblPedido == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblPedido.IdUsuario);
            return View(tblPedido);
        }

        // POST: TblPedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPedido,IdUsuario,MontoTotal,Telefono,Direccion,Fecha")] TblPedido tblPedido)
        {
            if (id != tblPedido.IdPedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblPedidoExists(tblPedido.IdPedido))
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
            ViewData["IdUsuario"] = new SelectList(_context.TblUsuarios, "IdUsuario", "IdUsuario", tblPedido.IdUsuario);
            return View(tblPedido);
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
