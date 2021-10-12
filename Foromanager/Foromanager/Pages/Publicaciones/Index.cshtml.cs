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
    public class IndexModel : PageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public IndexModel(Foromanager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Publicacion> Publicacion { get;set; }

        public async Task OnGetAsync()
        {
            Publicacion = await _context.Publicacion
                .Include(p => p.Foro).ToListAsync();
        }
    }
}
