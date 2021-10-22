namespace Foromanager.Models
{
	public enum Reaccion
	{
		Like,Love,Haha,Wow,Sad,Angry
	}
	public class Publicacion
	{
		public int PublicacionId {get; set;}
		//clave externa
		public int ForoId {get; set;}

		public string Usuario {get;set;}
		
		public string Titulo {get;set;}
		public string Descripcion {get;set;}
		public Foro Foro {get;set;}
		public Reaccion? Reaccion {get;set;}
		
	}
}