using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Foromanager.Models
{
    public class Usuario : IdentityUser
    {
        public byte[] FotodePerfil { get; set; }
        [Display(Name = "Nombre"), Required(ErrorMessage = "Nombre Requerido")]
        public string Nombre { get; set; }
        [Display(Name = "Apellido"), Required(ErrorMessage = "Apellido Requerido")]
        public string Apellido { get;set; }
    }
}
