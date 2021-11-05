using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Foromanager.Data;
using Foromanager.Models;

namespace Foromanager.Pages.img
{
    public class Crear_ImagenModel : PageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public Crear_ImagenModel(Foromanager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Imagenes Imagenes { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Imagenes.Add(Imagenes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
