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
    public enum Acciones
    {
        postear,
        aprobar,
        descartar,
        darlike,
        dardislike
    }
    
    public class DetailsModel : DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public DetailsModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService,UserManager<IdentityUser> userManager): base(context,authorizationService,userManager)
        {
            _context = context;
        }

        public Foro Foro { get; set; }
        [BindProperty]
        public Publicacion Publicacion { get; set; }

        public Acciones acciones {get;set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            
            Foro = await _context.Foro
                        .Include(s=>s.Publicaciones)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(m=>m.ForoId == id);


            if (id == null || Foro == null)
            {
                return NotFound();
            }
            
            var isAuthorizated = User.IsInRole(Constants.ForumManagersRole) || User.IsInRole(Constants.ForumAdministratorsRole);
            var currentUserId = UserManager.GetUserId(User);
            
            if(!isAuthorizated && currentUserId!=Foro.OwnerID && Foro.Status != ForumStatus.Approved)
            {
                return Forbid();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Foro = await _context.Foro.FirstOrDefaultAsync(m=>m.ForoId==id);
            //Publicacion = await _context.Publicacion.FirstOrDefaultAsync(p=>m.PublicacionId==idp);
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if(Foro == null)
            {
                return NotFound();
            }

            var forumOperation = (Foro.Status == ForumStatus.Approved)? ForumOperations.Approve : ForumOperations.Reject;
            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,forumOperation);

            if (!isAuthorizated.Succeeded && UserManager.GetUserId(User)!=Foro.OwnerID && Foro.Status != ForumStatus.Approved)
            {
                return Forbid();
            }
            switch(acciones)
            {
                case Acciones.aprobar:
                    Foro.Status = ForumStatus.Approved;
                    break;
                case Acciones.descartar:
                    Foro.Status = ForumStatus.Rejected;
                    break;
                case Acciones.postear:
                    Publicacion.ForoId = id;
                    Publicacion.Usuario = User.Identity.Name;
                    _context.Publicacion.Add(Publicacion);
                    break;
                case Acciones.darlike:
                    Publicacion.Likes++;
                    
                    break;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
