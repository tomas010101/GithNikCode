namespace Foromanager.Models
{
	public class Reaccion
	{
		public int ReaccionID {get;set;}
		public int PublicacionID {get;set;}
		public Publicacion Publicacion {get;set;}
		public bool Like {get;set;}
		public bool DisLike {get;set;}
	}
}