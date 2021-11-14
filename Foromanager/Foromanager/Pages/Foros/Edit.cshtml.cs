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
using Foromanager.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Foromanager.Pages.Foros
{
    public class EditModel : DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public EditModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<Usuario> userManager): base(context,authorizationService,userManager)
        {
            _context = context;
        }

        [BindProperty]
        public Foro Foro { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Foro = await _context.Foro.FirstOrDefaultAsync(m => m.ForoId == id);

            if (id == null)
            {
                return NotFound();
            }

            if (Foro == null)
            {
                return NotFound();
            }
            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Update);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var foro = await _context.Foro.AsNoTracking().FirstOrDefaultAsync(m=>m.ForoId == id);

            if(foro ==null)
            {
                return NotFound();
            }

            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Update);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }

            Foro.OwnerID = foro.OwnerID;
            _context.Attach(Foro).State = EntityState.Modified;

            if(Foro.Status == ForumStatus.Aprobado)
            {
                var canApprove = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Approve);

                if(!canApprove.Succeeded)
                {
                    Foro.Status = ForumStatus.Enviado;
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
