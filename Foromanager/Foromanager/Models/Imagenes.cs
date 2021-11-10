using System.ComponentModel.DataAnnotations.Schema;

namespace Foromanager.Models
{
    public class Imagenes
    {
        public int ImagenesId { get; set; }
        public string ImagenNombre { get; set; }
        public byte[] Imagen { get; set; }
        public Publicacion Publicacion {get;set;}

        [ForeignKey(nameof(Publicacion))]
        public int PublicacionId {get;set;}
    }
}
