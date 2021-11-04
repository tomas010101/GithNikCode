using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Foromanager.Models
{
	public class Foro
	{
		public int ForoId {get; set;}

		public string OwnerID {get;set;}

		public string Nombre {get; set;}
		public string Descripcion {get; set;}
		public DateTime Fecha {get;set;}
		
		public ICollection<Publicacion> Publicaciones {get;set;}
		public ForumStatus Status {get;set;}
		public ICollection<Categoria> Categorias { get; set; }
		public byte[] ForoPerfil {get;set;}
	}

	public enum ForumStatus
	{
		Submitted,
		Approved,
		Rejected
	}
}