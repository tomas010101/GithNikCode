using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Foromanager.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Foromanager.Data;

namespace Foromanager.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ApplicationDbContext _dbcontext;

        public IndexModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ApplicationDbContext dbcontex)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbcontext = dbcontex;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Número de Tléfono")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Nombre de Usuario")]
            public string UserName { get; set; }
        }

        private async Task LoadAsync(Usuario user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                UserName = userName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se puede cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
        [BindProperty]
        public Usuario Usuario { get; set; }
        [BindProperty]
        public IFormFile ImgCargaUsuario { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var archivoUsuario = HttpContext.Request.Form.Files.FirstOrDefault();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"No se puede cargar el usuario con ID '{_userManager.GetUserId(User)}'.");
            }

            if (ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Error inesperado al intentar establecer un número de teléfono.";
                    return RedirectToPage();
                }
            }
            var usernameresult = await _userManager.SetUserNameAsync(user, Input.UserName);
            if (!usernameresult.Succeeded)
            {
                StatusMessage = "Error inesperado al intentar establecer un nuevo nombre de usuario.";
                return RedirectToPage();
            }
            Usuario imagen = null;

            if (archivoUsuario != null)
            {
                imagen = new Usuario();
                using (var bReader = new BinaryReader(archivoUsuario.OpenReadStream()))
                {
                    user.FotodePerfil = bReader.ReadBytes((int)archivoUsuario.Length);
                }
            }
            await _dbcontext.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Tu perfil ha sido actualizado";
            return RedirectToPage();
        }
    }
}
