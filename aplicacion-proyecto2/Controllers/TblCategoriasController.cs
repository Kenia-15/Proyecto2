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
    public class TblCategoriasController : Controller
    {
        private readonly db_carritoContext _context;

        public TblCategoriasController(db_carritoContext context)
        {
            _context = context;
        }

        // GET: TblCategorias
        public async Task<IActionResult> Index(int idU)
        {
            return _context.TblCategorias != null ? 
            View(await _context.TblCategorias.ToListAsync()) :
            Problem("Entity set 'db_carritoContext.TblCategorias'  is null.");
        }

        // GET: TblCategorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblCategorias == null)
            {
                return NotFound();
            }

            var tblCategoria = await _context.TblCategorias
                .FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (tblCategoria == null)
            {
                return NotFound();
            }

            return View(tblCategoria);
        }

        // GET: TblCategorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TblCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria,Categoria,Estado")] TblCategoria tblCategoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblCategoria);
        }

        // GET: TblCategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblCategorias == null)
            {
                return NotFound();
            }

            var tblCategoria = await _context.TblCategorias.FindAsync(id);
            if (tblCategoria == null)
            {
                return NotFound();
            }
            return View(tblCategoria);
        }

        // POST: TblCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoria,Categoria,Estado")] TblCategoria tblCategoria)
        {
            if (id != tblCategoria.IdCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCategoriaExists(tblCategoria.IdCategoria))
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
            return View(tblCategoria);
        }

        // GET: TblCategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblCategorias == null)
            {
                return NotFound();
            }

            var tblCategoria = await _context.TblCategorias
                .FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (tblCategoria == null)
            {
                return NotFound();
            }

            return View(tblCategoria);
        }

        // POST: TblCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblCategorias == null)
            {
                return Problem("Entity set 'db_carritoContext.TblCategorias'  is null.");
            }
            var tblCategoria = await _context.TblCategorias.FindAsync(id);
            if (tblCategoria != null)
            {
                _context.TblCategorias.Remove(tblCategoria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCategoriaExists(int id)
        {
          return (_context.TblCategorias?.Any(e => e.IdCategoria == id)).GetValueOrDefault();
        }
    }
}
