using System.Collections.Generic;

namespace Foromanager.Models
{
	public class Reaccion
	{
		public int ReaccionId {get;set;}
		public int PublicacionId {get;set;}
		public ICollection<Publicacion> Publicaciones{get;set;}
		public bool Like {get;set;}
		public bool DisLike {get;set;}
	}
}