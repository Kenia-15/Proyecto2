using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using aplicacion_proyecto2.Models;

namespace aplicacion_proyecto.Controllers
{
    public class PersonasController : Controller
    {
        private readonly db_carritoContext _context;

        public PersonasController(db_carritoContext context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var p_busesContext = _context.TblPersonas.Include(t => t.IdMetodoPagoNavigation);
            return View(await p_busesContext.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TblPersonas == null)
            {
                return NotFound();
            }

            var tblPersona = await _context.TblPersonas
                .Include(t => t.IdMetodoPagoNavigation)
                .FirstOrDefaultAsync(m => m.IdPersona.Equals(id));
            if (tblPersona == null)
            {
                return NotFound();
            }

            return View(tblPersona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            ViewData["IdMetodoPago"] = new SelectList(_context.TblMetodosPagos, "IdMetodoPago", "IdMetodoPago");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPersona,IdMetodoPago,NumeroIdentificacion,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido")] TblPersona tblPersona)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(tblPersona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMetodoPago"] = new SelectList(_context.TblMetodosPagos, "IdMetodoPago", "IdMetodoPago", tblPersona.IdMetodoPago);
            return View(tblPersona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            TblUsuario per = new TblUsuario();

            per = _context.TblUsuarios.FirstOrDefault(p => p.IdUsuario == id);

            if (per.IdPersona == null || _context.TblPersonas == null)
            {
                return NotFound();
            }

            var tblPersona = await _context.TblPersonas.FindAsync(per.IdPersona);
            if (tblPersona == null)
            {
                return NotFound();
            }
            ViewData["MetodosPago"] = new SelectList(_context.TblMetodosPagos, "IdMetodoPago", "MetodoPago", tblPersona.IdMetodoPago);

            TempData["Usuario"] = id;

            return View(tblPersona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,IdMetodoPago,NumeroIdentificacion,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido")] TblPersona tblPersona)
        {
            TblUsuario per = new TblUsuario();

            per = _context.TblUsuarios.FirstOrDefault(p => p.IdUsuario == id);

            if (per.IdPersona != tblPersona.IdPersona)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(tblPersona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblPersonaExists(tblPersona.IdPersona.ToString()))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            //return RedirectToAction(nameof(Index));
            ViewData["MetodosPago"] = new SelectList(_context.TblMetodosPagos, "IdMetodoPago", "MetodoPago", tblPersona.IdMetodoPago);

            TempData["Usuario"] = id;

            return RedirectToAction("Index", "TblCategorias", new { idU = id });
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TblPersonas == null)
            {
                return NotFound();
            }

            var tblPersona = await _context.TblPersonas
                .Include(t => t.IdMetodoPagoNavigation)
                .FirstOrDefaultAsync(m => m.IdPersona.Equals(id));
            if (tblPersona == null)
            {
                return NotFound();
            }

            return View(tblPersona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TblPersonas == null)
            {
                return Problem("Entity set 'p_busesContext.TblPersonas'  is null.");
            }
            var tblPersona = await _context.TblPersonas.FindAsync(id);
            if (tblPersona != null)
            {
                _context.TblPersonas.Remove(tblPersona);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblPersonaExists(string id)
        {
          return (_context.TblPersonas?.Any(e => e.IdPersona.Equals(id))).GetValueOrDefault();
        }
    }
}
