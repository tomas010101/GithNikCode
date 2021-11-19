using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foromanager.Models
{
	public class Publicacion
	{
		public int PublicacionId {get; set;}
		//clave externa
		public int ForoId {get; set;}

		public string Usuario {get;set;}

		[Display(Name = "Titulo"), Required(ErrorMessage = "Titulo Requerido")]
		public string Titulo {get;set;}
		[Display(Name = "Descripcion"), Required(ErrorMessage = "Descripcion Requerida")]
		public string Descripcion {get;set;}
		public Foro Foro {get;set;}
		public ICollection<Reaccion> Reacciones {get;set;}
		public Imagenes Imagen {get;set;}
		public DateTime Fecha {get;set;}
	}
}