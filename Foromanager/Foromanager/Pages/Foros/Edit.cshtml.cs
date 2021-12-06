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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Foromanager.Pages.Foros
{
    public class EditModel : DI_BasePageModel 
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public EditModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<Usuario> userManager): base(context,authorizationService,userManager)
        {
            _context = context;
        }

        public Foro Foro { get; set; }

        [BindProperty]
        public IFormFile ImgCargaForo { get; set; }

        [BindProperty]
        public IFormFile ImgCargaBanner { get; set; }

        [BindProperty]
        public string Nombre {get; set;}

        [BindProperty]
        public string Descripcion { get; set; }

        [BindProperty]
        public int IdForo {get; set;}

        public string Categorias {get;set;}

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Foro = await _context.Foro.Include(c=>c.Categorias).FirstOrDefaultAsync(m => m.ForoId == id);
            
            foreach(var c in Foro.Categorias)
            {
                Categorias+=c.CategoriaNombre+"-";
            }
        
            if (id == null)
            {
                return NotFound();
            }

            if (Foro == null)
            {
                return NotFound();
            }

            Nombre = Foro.Nombre;
            Descripcion = Foro.Descripcion;

            IdForo = Foro.ForoId;

            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Update);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }
            
            return Page();
        }

       
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(Foro == null)
                Foro = await _context.Foro.FirstOrDefaultAsync(f => f.ForoId == IdForo);

            var ArchivoDeForo = HttpContext.Request.Form.Files.FirstOrDefault();

            if(Foro ==null)
            {
                return NotFound();
            }

            if (ArchivoDeForo != null)
            {
                using (var bReader = new BinaryReader(ArchivoDeForo.OpenReadStream()))
                {
                    Foro.ForoPerfil = bReader.ReadBytes((int)ArchivoDeForo.Length);
                }
            }      
            
            var ArchivoDeForoBanner = HttpContext.Request.Form.Files.FirstOrDefault();

            if (ArchivoDeForoBanner != null)
            {
                using (var bReader = new BinaryReader(ArchivoDeForoBanner.OpenReadStream()))
                {
                    Foro.Forobanner = bReader.ReadBytes((int)ArchivoDeForoBanner.Length);
                }
            }

            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Update);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }

            Foro.Nombre = Nombre;
            Foro.Descripcion = Descripcion;

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
