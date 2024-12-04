using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion_proyecto2.Models;

namespace aplicacion_proyecto2.Controllers
{
    public class TblDetallePedidoesController : Controller
    {
        private readonly db_carritoContext _context;
        public IConfiguration Configuration { get; }

        public TblDetallePedidoesController(db_carritoContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: TblDetallePedidoes
        public async Task<IActionResult> Index()
        {
            var db_carritoContext = _context.TblDetallePedidos.Include(t => t.IdDetalleProductoNavigation).Include(t => t.IdPedidoNavigation);
            return View(await db_carritoContext.ToListAsync());
        }

        // GET: TblDetallePedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblDetallePedidos == null)
            {
                return NotFound();
            }

            var tblDetallePedido = await _context.TblDetallePedidos
                .Include(t => t.IdDetalleProductoNavigation)
                .Include(t => t.IdPedidoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetallePedido == id);
            if (tblDetallePedido == null)
            {
                return NotFound();
            }

            return View(tblDetallePedido);
        }

        // GET: TblDetallePedidoes/Create
        public IActionResult Create()
        {
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto");
            ViewData["IdPedido"] = new SelectList(_context.TblPedidos, "IdPedido", "IdPedido");

            return View();
        }


        // POST: TblDetallePedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetallePedido,IdPedido,IdDetalleProducto,Cantidad,TotalPedido")] TblDetallePedido tblDetallePedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblDetallePedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto", tblDetallePedido.IdDetalleProducto);
            ViewData["IdPedido"] = new SelectList(_context.TblPedidos, "IdPedido", "IdPedido", tblDetallePedido.IdPedido);
            return View(tblDetallePedido);
        }

        // GET: TblDetallePedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblDetallePedidos == null)
            {
                return NotFound();
            }

            var tblDetallePedido = await _context.TblDetallePedidos.FindAsync(id);
            if (tblDetallePedido == null)
            {
                return NotFound();
            }
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto", tblDetallePedido.IdDetalleProducto);
            ViewData["IdPedido"] = new SelectList(_context.TblPedidos, "IdPedido", "IdPedido", tblDetallePedido.IdPedido);
            return View(tblDetallePedido);
        }

        // POST: TblDetallePedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetallePedido,IdPedido,IdDetalleProducto,Cantidad,TotalPedido")] TblDetallePedido tblDetallePedido)
        {
            if (id != tblDetallePedido.IdDetallePedido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblDetallePedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblDetallePedidoExists(tblDetallePedido.IdDetallePedido))
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
            ViewData["IdDetalleProducto"] = new SelectList(_context.TblDetalleProductos, "IdDetalleProducto", "IdDetalleProducto", tblDetallePedido.IdDetalleProducto);
            ViewData["IdPedido"] = new SelectList(_context.TblPedidos, "IdPedido", "IdPedido", tblDetallePedido.IdPedido);
            return View(tblDetallePedido);
        }

        // GET: TblDetallePedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblDetallePedidos == null)
            {
                return NotFound();
            }

            var tblDetallePedido = await _context.TblDetallePedidos
                .Include(t => t.IdDetalleProductoNavigation)
                .Include(t => t.IdPedidoNavigation)
                .FirstOrDefaultAsync(m => m.IdDetallePedido == id);
            if (tblDetallePedido == null)
            {
                return NotFound();
            }

            return View(tblDetallePedido);
        }

        // POST: TblDetallePedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblDetallePedidos == null)
            {
                return Problem("Entity set 'db_carritoContext.TblDetallePedidos'  is null.");
            }
            var tblDetallePedido = await _context.TblDetallePedidos.FindAsync(id);
            if (tblDetallePedido != null)
            {
                _context.TblDetallePedidos.Remove(tblDetallePedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblDetallePedidoExists(int id)
        {
          return (_context.TblDetallePedidos?.Any(e => e.IdDetallePedido == id)).GetValueOrDefault();
        }
    }
}
