using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Foromanager.Data;
using Foromanager.Models;

namespace Foromanager.Pages.Publicaciones
{
    public class DetailsModel : PageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public DetailsModel(Foromanager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
