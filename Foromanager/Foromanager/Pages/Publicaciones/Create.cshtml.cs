using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Foromanager.Data;
using Foromanager.Models;

namespace Foromanager.Pages.Publicaciones
{
    public class CreateModel : PageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public CreateModel(Foromanager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ForoId"] = new SelectList(_context.Foro, "ForoId", "ForoId");
            return Page();
        }

        [BindProperty]
        public Publicacion Publicacion { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Publicacion.Add(Publicacion);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
