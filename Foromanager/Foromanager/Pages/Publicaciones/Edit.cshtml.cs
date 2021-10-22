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

namespace Foromanager.Pages.Publicaciones
{
    public class EditModel : PageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public EditModel(Foromanager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            //ViewData["ForoId"] = new SelectList(_context.Foro, "ForoId", "ForoId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var p = await _context.Publicacion.FindAsync(id);

            if(p ==null)
            {
                return NotFound();
            }

            if(await TryUpdateModelAsync<Publicacion>(p,"publicacion",p => p.Titulo, p => p.Descripcion))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("../Foros/Index");
            }
            return Page();
            /*
            _context.Attach(Publicacion).State = EntityState.Modified;
            
            if (!ModelState.IsValid)
            {
                return Page();
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
            */
        }

        private bool PublicacionExists(int id)
        {
            return _context.Publicacion.Any(e => e.PublicacionId == id);
        }
    }
}
