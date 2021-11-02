namespace Foromanager.Models
{
	public class Reaccion
	{
		public int ReaccionId {get;set;}
		public int PublicacionId {get;set;}
		public Publicacion Publicacion {get;set;}
		public bool Like {get;set;}
		public bool DisLike {get;set;}
	}
}