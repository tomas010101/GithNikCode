namespace Foromanager.Models
{
    public class Imagenes
    {
        public int ImagenesID { get; set; }
        public string ImagenNombre { get; set; }
        public byte[] Imagen { get; set; }
        public int PublicacionID {get;set;}
    }
}
