using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Foromanager.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Foromanager.Models;
using Foromanager.Data;

namespace Foromanager.Pages
{
    [AllowAnonymous]
    public class IndexModel : Foros.DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger,Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<Usuario> userManager):base(context,authorizationService,userManager)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Foro> Foros { get;set; }
        public int Usuarios { get; set; }

        public async Task OnGetAsync()
        {
            await Task.Run(()=>
            { 
                IQueryable<Foro> forosLista = from p in _context.Foro select p;
                foreach (var usuario in _context.Users.ToList())
                {
                    Usuarios++;
                }

                var isAuthorizated = User.IsInRole(Constants.ForumManagersRole) || User.IsInRole(Constants.ForumAdministratorsRole);
                
                var currentUserId = UserManager.GetUserId(User);

                if(!isAuthorizated)
                {
                    forosLista = forosLista.Where(f => f.Status == ForumStatus.Aprobado && f.OwnerID == currentUserId);
                }
                Foros = forosLista.ToList();
            });
        }
    }
}
