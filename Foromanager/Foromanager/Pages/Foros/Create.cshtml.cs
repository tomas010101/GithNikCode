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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Foromanager.Pages.Foros
{
    public class CreateModel : DI_BasePageModel
    {
        private readonly Foromanager.Data.ApplicationDbContext _context;

        public CreateModel(Foromanager.Data.ApplicationDbContext context,IAuthorizationService authorizationService, UserManager<Usuario> userManager): base(context,authorizationService,userManager)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Foro Foro { get; set; }
        [BindProperty]
        public string Categorias { get; set; }
        [BindProperty]
        public IFormFile ImgCargaForo { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var archivoForo = HttpContext.Request.Form.Files.FirstOrDefault();
            Foro imagen = null;


            if (archivoForo != null)
            {
                imagen = new Foro();
                using (var bReader = new BinaryReader(archivoForo.OpenReadStream()))
                {
                    Foro.ForoPerfil = bReader.ReadBytes((int)archivoForo.Length);


                }
            }



            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                string[] categoriaLista = Categorias.Split('-');

                foreach (var c in categoriaLista)
                {
                    Foro.Categorias = new List<Categoria>();
                    Foro.Categorias.Add(new Categoria() { CategoriaNombre = c });
                }
            }
            catch
            {
                ViewData["Message"] = "Alguno de los Campos esta vacio, por favor completelos todos";
            }
            
            Foro.OwnerID = UserManager.GetUserId(User);
            var isAuthorizated = await AuthorizationService.AuthorizeAsync(User,Foro,ForumOperations.Create);

            if(!isAuthorizated.Succeeded)
            {
                return Forbid();
            }
            Foro.Status=ForumStatus.Aprobado;
            Foro.Fecha = DateTime.Now;
            _context.Foro.Add(Foro);

            await _context.SaveChangesAsync();


            return RedirectToPage("./Index");
        }
    }
}
