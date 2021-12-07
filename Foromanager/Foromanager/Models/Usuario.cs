using Microsoft.AspNetCore.Identity;

namespace Foromanager.Models
{
    public class Usuario : IdentityUser
    {
        public byte[] FotodePerfil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get;set; }
    }
}
