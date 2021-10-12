using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Foromanager.Data;
using Foromanager.Models;
using Foromanager.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Foromanager.Pages.Foros
{
    public class CreateModel : DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public CreateModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<IdentityUser> userManager): base(context,authorizationService,userManager)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Foro Foro { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Foro.OwnerID = UserManager.GetUserId(User);
            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Create);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }

            _context.Foro.Add(Foro);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
