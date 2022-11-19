using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerFedexMotos.Data;
using TallerFedexMotos.Models;
using TallerFedexMotos.ModelsView;

namespace TallerFedexMotos.Controllers
{
    public class MotosEnVentaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotosEnVentaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MotosEnVenta       
        public async Task<IActionResult> Index(int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                cantReg = _context.motosEnVenta.Count(),
                pagActual = pagina,
                regXpag = 5
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.motosEnVenta
                .Include(x => x.moto)
                .ThenInclude(x => x.modelo)
                .ThenInclude(x => x.marca)
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            return View(await datosAmostrar.ToListAsync());
        }

        // GET: MotosEnVenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motoEnVenta = await _context.motosEnVenta
                .Include(m => m.moto)
                .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motoEnVenta == null)
            {
                return NotFound();
            }
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnVenta.motoId);       

            return View(motoEnVenta);
        }

        // GET: MotosEnVenta/Create
        public IActionResult Create()
        {           
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName");
            return View();

        }

        // POST: MotosEnVenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,motoId,precio")] MotoEnVenta motoEnVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(motoEnVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["motoId"] = new SelectList(_context.motos, "Id", "Id", motoEnVenta.motoId);
            return View(motoEnVenta);
        }

        // GET: MotosEnVenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motoEnVenta = await _context.motosEnVenta.FindAsync(id);
            if (motoEnVenta == null)
            {
                return NotFound();
            } 
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName",
                                 motoEnVenta.motoId);
            return View();
        }

        // POST: MotosEnVenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,motoId,precio")] MotoEnVenta motoEnVenta)
        {
            if (id != motoEnVenta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(motoEnVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotoEnVentaExists(motoEnVenta.Id))
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
            ViewData["motoId"] = new SelectList(_context.motos, "Id", "Id", motoEnVenta.motoId);
            return View(motoEnVenta);
        }

        // GET: MotosEnVenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motoEnVenta = await _context.motosEnVenta
                .Include(m => m.moto)
                .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motoEnVenta == null)
            {
                return NotFound();
            }

            return View(motoEnVenta);
        }

        // POST: MotosEnVenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var motoEnVenta = await _context.motosEnVenta.FindAsync(id);
            _context.motosEnVenta.Remove(motoEnVenta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotoEnVentaExists(int id)
        {
            return _context.motosEnVenta.Any(e => e.Id == id);
        }
    }
}
