using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Foromanager.Data;
using Foromanager.Models;
using Foromanager.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Foromanager.Pages.Foros
{
    public class DeleteModel : DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;
        
        public DeleteModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService,UserManager<Usuario> userManager): base(context,authorizationService,userManager)
        {
            _context = context;
        }

        [BindProperty]
        public Foro Foro { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Foro = await _context.Foro.FirstOrDefaultAsync(m=>m.ForoId==id);

            if (id == null)
            {
                return NotFound();
            }

            if (Foro == null)
            {
                return NotFound();
            }

            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Delete);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var foro = await _context.Foro.FirstOrDefaultAsync(m=>m.ForoId==id);

            if (id == null)
            {
                return NotFound();
            }
            if(Foro == null)
            {
                return NotFound();
            }

            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Delete);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }

            _context.Foro.Remove(foro);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
