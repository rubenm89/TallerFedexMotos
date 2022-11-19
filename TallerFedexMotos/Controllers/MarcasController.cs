using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TallerFedexMotos.Data;
using TallerFedexMotos.Models;
using TallerFedexMotos.ModelsView;

namespace TallerFedexMotos.Controllers
{
    [Authorize]
    public class MarcasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public MarcasController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Marcas
        
        public async Task<IActionResult> Index(int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                cantReg = _context.marcas.Count(),
                pagActual = pagina,
                regXpag = 5
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.marcas
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            return View(await datosAmostrar.ToListAsync());
        }

        public async Task<IActionResult> Importar()
        {
            var archivos = HttpContext.Request.Form.Files;
            List<Marca> MarcasArch = new List<Marca>();
            if (archivos != null && archivos.Count > 0)
            {
                var archivo = archivos[0];
                if (archivo.Length > 0)
                {
                    var pathDestino = Path.Combine(env.WebRootPath, "importaciones");
                    if (archivo.Length > 0)
                    {
                        var archivoDestino = Guid.NewGuid().ToString();
                        archivoDestino += archivoDestino.Replace("-", "");
                        archivoDestino += Path.GetExtension(archivo.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivo.CopyTo(filestream);
                        }

                        using (var file = new FileStream(rutaDestino, FileMode.Open))
                        {
                            List<string> renglones = new List<string>();
                            

                            StreamReader fileContent = new StreamReader(file, System.Text.Encoding.Default);
                            do
                            {
                                renglones.Add(fileContent.ReadLine());
                            }
                            while (!fileContent.EndOfStream);

                            if (renglones.Count() > 0)
                            {
                                foreach (var renglon in renglones)
                                {
                                    string[] data = renglon.Split(';');
                                    if (data.Length == 1)
                                    {
                                        Marca marcaImportada = new Marca();
                                        marcaImportada.nombreMarca = data[0].Trim();
                                        MarcasArch.Add(marcaImportada);
                                    }
                                }
                                if (MarcasArch.Count > 0)
                                {
                                    _context.AddRange(MarcasArch);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Marcas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marcas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marcas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombreMarca")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marca);
        }

        // GET: Marcas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        // POST: Marcas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombreMarca")] Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.Id))
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
            return View(marca);
        }

        // GET: Marcas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marca = await _context.marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marcas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marca = await _context.marcas.FindAsync(id);
            _context.marcas.Remove(marca);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(int id)
        {
            return _context.marcas.Any(e => e.Id == id);
        }
    }
}
