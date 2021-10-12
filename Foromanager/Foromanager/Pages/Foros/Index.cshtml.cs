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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Foromanager.Pages.Foros
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<IdentityUser> userManager, IConfiguration configuration):base(context,authorizationService,userManager)
        {
            _context = context;
            Configuration = configuration;
        }

        public IList<Foro> Foro { get;set; }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<Foro> Foros { get; set; }
        
        public async Task OnGetAsync(string sortOrder,string currentFilter,string searchString,int? pageIndex)
        {
            await Task.Run(async ()=>
            { 
                NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                DateSort = sortOrder == "Date" ? "date_desc" : "Date";
                CurrentFilter = searchString;

                if (searchString != null)
                {
                    pageIndex = 1;
                }
                else
                {
                    searchString = currentFilter;
                }
                IQueryable<Foro> ForosIQ = from f in _context.Foro.Include(s=>s.Publicaciones).AsNoTracking() select f;
                if(!String.IsNullOrEmpty(searchString))
                {
                    ForosIQ = ForosIQ.Where(f => f.Nombre.ToUpper().Contains(searchString.ToUpper()) || f.Categoria.ToUpper().Contains(searchString.ToUpper()));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        ForosIQ = ForosIQ.OrderByDescending(f => f.Nombre);
                        break;
                    case "Date":
                        ForosIQ = ForosIQ.OrderBy(f => f.Fecha);
                        break;
                    case "categoria":
                        ForosIQ = ForosIQ.OrderByDescending(f => f.Categoria);
                        break;
                    default:
                        ForosIQ = ForosIQ.OrderBy(f => f.Nombre);
                        break;
                }

                var isAuthorizated = User.IsInRole(Constants.ForumManagersRole) || User.IsInRole(Constants.ForumAdministratorsRole);
                
                var currentUserId = UserManager.GetUserId(User);

                if(!isAuthorizated)
                {
                    ForosIQ = ForosIQ.Where(f => f.Status == ForumStatus.Approved || f.OwnerID == currentUserId);
                }

               /* Foro = await _context.Foro
                            .Include(s=>s.Publicaciones)
                            .AsNoTracking()
                            .ToListAsync();
                */
                var pageSize = Configuration.GetValue("PageSize", 4);
                Foros = await PaginatedList<Foro>.CreateAsync(ForosIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
            });
        }
    }
}
