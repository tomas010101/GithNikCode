using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Foromanager.Data;
using Foromanager.Models;
using Foromanager.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Foromanager.Pages.Foros
{
	public enum Acciones
	{
		postear,
		like,
		dislike,
		aprobar,
		descartar
	}

	public class DetailsModel : DI_BasePageModel
	{
		private readonly Foromanager.Data.ApplicationDbContext _context;

		public DetailsModel(Foromanager.Data.ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<Usuario> userManager) : base(context, authorizationService, userManager)
		{
			_context = context;
		}
		[BindProperty]
		public Publicacion Publicacion { get; set; }

		public Foro Foro { get; set; }

		[BindProperty]
		public Acciones acciones { get; set; }

		public int PublicacionId { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			Foro = await _context.Foro
						.Include(s => s.Publicaciones)
						.AsNoTracking()
						.FirstOrDefaultAsync(m => m.ForoId == id);

			if (id == null || Foro == null)
			{
				return NotFound();
			}

			var isAuthorizated = User.IsInRole(Constants.ForumManagersRole) || User.IsInRole(Constants.ForumAdministratorsRole);
			var currentUserId = UserManager.GetUserId(User);

			if (!isAuthorizated && currentUserId != Foro.OwnerID && Foro.Status != ForumStatus.Aprobado)
			{
				return Forbid();
			}
			return Page();
		}

		[BindProperty]
		public IFormFile ImgCarga { get; set; }

		public async Task<IActionResult> OnPostAsync(int id)
		{
			var archivo = HttpContext.Request.Form.Files.FirstOrDefault();
			Imagenes imagen = null;

			if (archivo != null)
			{
				imagen = new Imagenes();
				using (var bReader = new BinaryReader(archivo.OpenReadStream()))
				{
					imagen.Imagen = bReader.ReadBytes((int)archivo.Length);
					imagen.ImagenNombre = archivo.Name;
				}
			}

			Foro = await _context.Foro.FirstOrDefaultAsync(m => m.ForoId == id);

			if (Foro == null)
			{
				return NotFound();
			}

			var forumOperation = (Foro.Status == ForumStatus.Aprobado) ? ForumOperations.Approve : ForumOperations.Reject;
			var isAuthorizated = await AuthorizationService.AuthorizeAsync(User, Foro, forumOperation);

			if (!isAuthorizated.Succeeded && UserManager.GetUserId(User) != Foro.OwnerID && Foro.Status != ForumStatus.Aprobado)
			{
				return Forbid();
			}

			switch (acciones)
			{
				case Acciones.like:
					//Publicacion = await _context.Publicacion.Include(r => r.Reacciones).FirstOrDefaultAsync(p => p.PublicacionId == PublicacionId);
					Publicacion = await _context.Publicacion.AsNoTracking().Include(r => r.Reacciones).FirstOrDefaultAsync(p => p.PublicacionId == PublicacionId);
					if (!Publicacion.Reacciones.Any())
					{
						Publicacion.Reacciones = new List<Reaccion>();						
					}
					Publicacion.Reacciones.Add(new Reaccion() { Like = true, Usuario = User.Identity.Name });
					_context.Attach(Publicacion).State = EntityState.Modified;
					break;
				case Acciones.dislike:
					Publicacion = await _context.Publicacion.AsNoTracking().Include(r => r.Reacciones).FirstOrDefaultAsync(p => p.PublicacionId == PublicacionId);
					if (!Publicacion.Reacciones.Any())
					{
						Publicacion.Reacciones = new List<Reaccion>();						
					}
					Publicacion.Reacciones.Add(new Reaccion() { DisLike = true, Usuario = User.Identity.Name });
					_context.Attach(Publicacion).State = EntityState.Modified;
					break;
				case Acciones.aprobar:
					Foro.Status = ForumStatus.Aprobado;
					break;
				case Acciones.descartar:
					Foro.Status = ForumStatus.Rechazado;
					break;
				case Acciones.postear:
					Publicacion.ForoId = id;
					Publicacion.Usuario = User.Identity.Name.Split('-')[0] + " " + User.Identity.Name.Split('-')[1];
					Publicacion.Imagen = imagen;
					Publicacion.Fecha = DateTime.Now;
					_context.Publicacion.Add(Publicacion);
					break;
			}

			await _context.SaveChangesAsync();

			return RedirectToPage("Details", new { id = id });
		}
	}
}