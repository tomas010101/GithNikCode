using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foromanager.Models
{
	public enum Reaccion
	{
		Like,Dislike
	}
	public class Publicacion
	{
		public int PublicacionId {get; set;}
		//clave externa
		public int ForoId {get; set;}

		public string Usuario {get;set;}
		
		[Required]
		public string Titulo {get;set;}
		[Required]
		public string Descripcion {get;set;}
		
		public Foro Foro {get;set;}
		public Reaccion? Reaccion {get;set;}
		public int Likes {get;set;}
		public int Dislikes {get;set;}
	}
}