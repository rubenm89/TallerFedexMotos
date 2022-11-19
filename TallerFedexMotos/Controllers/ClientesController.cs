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
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Clientes
        
        public async Task<IActionResult> Index(string busquedaNombre, string busquedaApellido, int? motoId, int pagina = 1)
        {
            var appDBcontext = _context.clientes.Include(a => a).Select(a => a);
            
            if (!string.IsNullOrEmpty(busquedaNombre))
            {
                appDBcontext = appDBcontext.Where(e => e.nombre.Contains(busquedaNombre));                
            }

            if (!string.IsNullOrEmpty(busquedaApellido))
            {
                appDBcontext = appDBcontext.Where(e => e.apellido.Contains(busquedaApellido));               
            }

            if (motoId.HasValue)
            {
                appDBcontext = appDBcontext.Where(e => e.motoId == motoId);              
            }
            ViewData["motoId"] = new SelectList(_context.modelos, "Id", "nombreModelo", motoId);
                                    
            paginador paginador = new paginador()
            {
                cantReg = _context.clientes.Count(),
                pagActual = pagina,
                regXpag = 5
            };
            ViewData["paginador"] = paginador;

            var consulta = _context.clientes.Include(a => a.moto).Select(a => a);

            if (!string.IsNullOrEmpty(busquedaNombre))
            {
                consulta = appDBcontext.Where(e => e.nombre.Contains(busquedaNombre));               
            }
            if (!string.IsNullOrEmpty(busquedaApellido))
            {
                consulta = appDBcontext.Where(e => e.apellido.Contains(busquedaApellido));               
            }
            if (motoId.HasValue)
            {
                consulta = consulta.Where(e => e.motoId == (motoId+1));               
            }
                      
            paginador.cantReg = consulta.Count();
            
            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);
                       
            var datosAmostrarClientes = consulta
               .Include(x => x.moto)
                   .ThenInclude(x => x.modelo)
                       .ThenInclude(x => x.marca)
               .Skip((paginador.pagActual - 1) * paginador.regXpag)
               .Take(paginador.regXpag);
                       
            return View(await datosAmostrarClientes.ToListAsync());
        }        

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.clientes
                .Include(c => c.moto)
                 .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {           
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName");

            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,apellido,motoId")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["motoId"] = new SelectList(_context.motos, "Id", "Id", cliente.motoId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var motos = _context.motos.Include(x => x.modelo).ThenInclude(x => x.marca);

            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["motoId"] = new SelectList(motos, "Id", "Id", cliente.motoId);
            ViewData["motoId"] = new SelectList((from s in _context.motos select new { Id = s.Id, FullName = s.modelo.nombreModelo }),
                                 "Id",
                                 "FullName");           
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,apellido,motoId")] Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
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
            ViewData["motoId"] = new SelectList(_context.motos, "Id", "Id", cliente.motoId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.clientes
                .Include(c => c.moto)
                .ThenInclude(x => x.modelo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.clientes.FindAsync(id);
            _context.clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.clientes.Any(e => e.Id == id);
        }
    }
}
