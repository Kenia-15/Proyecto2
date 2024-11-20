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
    public class TblCarritoesController : Controller
    {
        private readonly db_carritoContext _context;

        public TblCarritoesController(db_carritoContext context)
        {
            _context = context;
        }

        // GET: TblCarritoes
        public async Task<IActionResult> Index()
        {
            var db_carritoContext = _context.TblCarritos.Include(t => t.IdDetalleProductoNavigation).Include(t => t.IdUsuarioNavigation);
            return View(await db_carritoContext.ToListAsync());
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
            List<TblListaDetalleCarrito> detalleCarrito = new List<TblListaDetalleCarrito>();
            List<TblCarrito> listCarrito = new List<TblCarrito>();
            TblListaDetalleCarrito carrito;

            //Se obtiene el carrito por usuario
            listCarrito = _context.TblCarritos.Where(p => p.IdUsuario == id).ToList();

            if (listCarrito.Count > 0)
            {
                foreach (var item in listCarrito)
                {
                    carrito = new TblListaDetalleCarrito();

                    carrito.Cantidad = item.Cantidad;

                    carrito.Color = _context.TblColores.FirstOrDefault(p => p.IdColor.Equals(item.IdDetalleProductoNavigation.IdColor)).ToString();
                }
            }

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
        public async Task<IActionResult> Delete(int? id)
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

        // POST: TblCarritoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
            return RedirectToAction(nameof(Index));
        }

        private bool TblCarritoExists(int id)
        {
          return (_context.TblCarritos?.Any(e => e.IdCarrito == id)).GetValueOrDefault();
        }
    }
}
