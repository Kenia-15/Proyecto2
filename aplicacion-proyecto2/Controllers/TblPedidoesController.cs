using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion_proyecto2.Models;
using System.Runtime.Intrinsics.Arm;

namespace aplicacion_proyecto2.Controllers
{
    public class TblPedidoesController : Controller
    {
        private readonly db_carritoContext _context;

        public TblPedidoesController(db_carritoContext context)
        {
            _context = context;
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
            TblCarrito listaCarrito = new TblCarrito();
            TblDetallePedido detalle = new TblDetallePedido();

            //se crea el id del registro

            try
            {
                //Se inserta la información en la tabla de pedidos
                tblPedido.Fecha = fechaActual;
                _context.Add(tblPedido);

                //Se inserta la información en la tabla de detalle de pedidos

                _context.Add(detalle);

                await _context.SaveChangesAsync();

                //Se elimina la información de la tabla carrito si se actualizó correctamente la de detalle de pedidos
                

                
                return RedirectToAction(nameof(Index));
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
