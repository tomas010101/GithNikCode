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
using Foromanager.Models;
using Foromanager.Data;

namespace Foromanager.Pages
{
    [AllowAnonymous]
    public class IndexModel : Foros.DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger,Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<IdentityUser> userManager):base(context,authorizationService,userManager)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Foro> Foros { get;set; }

        public async Task OnGetAsync()
        {
            await Task.Run(()=>
            { 
                var foros = from f in _context.Foro.ToList() select f;

                var isAuthorizated = User.IsInRole(Constants.ForumManagersRole) || User.IsInRole(Constants.ForumAdministratorsRole);
                
                var currentUserId = UserManager.GetUserId(User);

                if(!isAuthorizated)
                {
                    foros = foros.Where(f => f.Status == ForumStatus.Approved && f.OwnerID == currentUserId);
                }
                Foros = foros.ToList();
            });
        }
    }
}
