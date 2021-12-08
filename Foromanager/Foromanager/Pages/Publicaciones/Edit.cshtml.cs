using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foromanager.Data;
using Foromanager.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Foromanager.Pages.Publicaciones
{
    public class EditModel : PageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public EditModel(Foromanager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        Foro Foro { get; set; }
        [BindProperty]
        public Publicacion Publicacion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Publicacion = await _context.Publicacion
                .Include(p => p.Foro).FirstOrDefaultAsync(m => m.PublicacionId == id);

            if (Publicacion == null)
            {
                return NotFound();
            }
            ViewData["ForoId"] = new SelectList(_context.Foro, "ForoId", "ForoId");
            return Page();
        }

        [BindProperty]
        public IFormFile ImgCarga { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var publicacion = await _context.Publicacion.AsNoTracking().FirstOrDefaultAsync(m => m.PublicacionId == id);
            Publicacion.Fecha = publicacion.Fecha;
            Publicacion.Usuario = publicacion.Usuario;
            Publicacion.ForoId = publicacion.ForoId;
            _context.Attach(Publicacion).State = EntityState.Modified;

            if (ImgCarga != null)
            {
                Imagenes imagen = await _context.Imagenes.SingleOrDefaultAsync(i => i.PublicacionId == id);
                if (imagen == null)
                {
                    imagen = new Imagenes()
                    {
                        PublicacionId = id
                    };
                    _context.Add(imagen);
                }
                using (var bReader = new BinaryReader(ImgCarga.OpenReadStream()))
                {
                    imagen.Imagen = bReader.ReadBytes((int)ImgCarga.Length);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicacionExists(Publicacion.PublicacionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Foros/Index");
        }

        private bool PublicacionExists(int id)
        {
            return _context.Publicacion.Any(e => e.PublicacionId == id);
        }
    }
}
