using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerFedexMotos.Data;
using TallerFedexMotos.Models;
using TallerFedexMotos.ModelsView;

namespace TallerFedexMotos.Controllers
{
    [Authorize]
    public class MotosEnReparacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotosEnReparacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MotosEnReparacion
        
        public async Task<IActionResult> Index(int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                cantReg = _context.motosEnReparacion.Count(),
                pagActual = pagina,
                regXpag = 5
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.motosEnReparacion
                    .Include(x => x.moto)
                    .ThenInclude(x => x.modelo)
                    .ThenInclude(x => x.marca)
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            return View(await datosAmostrar.ToListAsync());
        }

        // GET: MotosEnReparacion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motoEnReparacion = await _context.motosEnReparacion
                .Include(m => m.moto)
                .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motoEnReparacion == null)
            {
                return NotFound();
            }
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnReparacion.motoId);

            return View(motoEnReparacion);
        }

        // GET: MotosEnReparacion/Create
        public IActionResult Create()
        {            
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName");
            return View();
        }

        // POST: MotosEnReparacion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,motoId,trabajoRealizado")] MotoEnReparacion motoEnReparacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(motoEnReparacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnReparacion.motoId);

            return View(motoEnReparacion);
        }

        // GET: MotosEnReparacion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motoEnReparacion = await _context.motosEnReparacion.FindAsync(id);
            if (motoEnReparacion == null)
            {
                return NotFound();
            }
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnReparacion.motoId);
            return View(motoEnReparacion);
        }

        // POST: MotosEnReparacion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,motoId,trabajoRealizado")] MotoEnReparacion motoEnReparacion)
        {
            if (id != motoEnReparacion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(motoEnReparacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotoEnReparacionExists(motoEnReparacion.Id))
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
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnReparacion.motoId);

            return View(motoEnReparacion);
        }

        // GET: MotosEnReparacion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motoEnReparacion = await _context.motosEnReparacion
                .Include(m => m.moto)
                    .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motoEnReparacion == null)
            {
                return NotFound();
            }
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnReparacion.motoId);

            return View(motoEnReparacion);
        }

        // POST: MotosEnReparacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motoEnReparacion = await _context.motosEnReparacion.FindAsync(id);
            _context.motosEnReparacion.Remove(motoEnReparacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotoEnReparacionExists(int id)
        {
            return _context.motosEnReparacion.Any(e => e.Id == id);
        }
    }
}
