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
		public Imagenes imagen = null;
		public async Task<IActionResult> OnPostAsync(int id)
		{
			
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

            //switch (acciones)
            //{
            //    case acciones.like:
            //        //publicacion = await _context.publicacion.include(r => r.reacciones).firstordefaultasync(p => p.publicacionid == publicacionid);
            //        publicacion = await _context.publicacion.asnotracking().include(r => r.reacciones).firstordefaultasync(p => p.publicacionid == publicacionid);
            //        if (!publicacion.reacciones.any())
            //        {
            //            publicacion.reacciones = new list<reaccion>();
            //        }
            //        publicacion.reacciones.add(new reaccion() { like = true, usuario = user.identity.name });
            //        _context.attach(publicacion).state = entitystate.modified;
            //        break;
            //    case acciones.dislike:
            //        publicacion = await _context.publicacion.asnotracking().include(r => r.reacciones).firstordefaultasync(p => p.publicacionid == publicacionid);
            //        if (!publicacion.reacciones.any())
            //        {
            //            publicacion.reacciones = new list<reaccion>();
            //        }
            //        publicacion.reacciones.add(new reaccion() { dislike = true, usuario = user.identity.name });
            //        _context.attach(publicacion).state = entitystate.modified;
            //        break;
            //    case acciones.aprobar:
            //        foro.status = forumstatus.aprobado;
            //        break;
            //    case acciones.descartar:
            //        foro.status = forumstatus.rechazado;
            //        break;
            //    case acciones.postear:
            //        publicacion.foroid = id;
            //        publicacion.usuario = user.identity.name.split('-')[0] + " " + user.identity.name.split('-')[1];
            //        publicacion.imagen = imagen;
            //        publicacion.fecha = datetime.now;
            //        _context.publicacion.add(publicacion);
            //        break;
            //}

            await _context.SaveChangesAsync();

			return RedirectToPage("Details", new { id = id });
		}
		public async Task<IActionResult> OnPostLike(int idp, int id)
		{
			Publicacion = await _context.Publicacion.AsNoTracking().Include(r => r.Reacciones).FirstOrDefaultAsync(p => p.PublicacionId == idp);
			if (!Publicacion.Reacciones.Any())
			{
				Publicacion.Reacciones = new List<Reaccion>();
			}
			if (Publicacion.Reacciones.Any(r => r.Like && r.Usuario == User.Identity.Name))
			{
				Reaccion r = Publicacion.Reacciones.SingleOrDefault(r => r.PublicacionId == idp);
				Publicacion.Reacciones.Remove(r);
				r.Like = false;
				Publicacion.Reacciones.Add(r);
				_context.Attach(Publicacion).State = EntityState.Modified;
			}
			if (!_context.Reaccion.Any(r => r.PublicacionId==idp && r.Usuario == User.Identity.Name))
			{
				Publicacion.Reacciones.Add(new Reaccion() { Like = true, Usuario = User.Identity.Name, PublicacionId = idp });
				_context.Attach(Publicacion).State = EntityState.Modified;
			}
			await _context.SaveChangesAsync();
			return RedirectToPage("Details", new { id = id });
		}
		public async Task<IActionResult> OnPostDisLike(int idp, int id)
		{
			Publicacion = await _context.Publicacion.AsNoTracking().Include(r => r.Reacciones).FirstOrDefaultAsync(p => p.PublicacionId == idp);
			if (!Publicacion.Reacciones.Any())
			{
				Publicacion.Reacciones = new List<Reaccion>();
			}
			if (Publicacion.Reacciones.Any(r => r.Like && r.Usuario == User.Identity.Name))
			{
				Reaccion r = Publicacion.Reacciones.SingleOrDefault(r => r.PublicacionId == idp);
				r.DisLike = false;
				Publicacion.Reacciones.Add(r);
				_context.Attach(Publicacion).State = EntityState.Modified;
			}
			if (!_context.Reaccion.Any(r => r.PublicacionId == idp && r.Usuario == User.Identity.Name))
			{
				Publicacion.Reacciones.Add(new Reaccion() { DisLike = true, Usuario = User.Identity.Name, PublicacionId = idp });
				_context.Attach(Publicacion).State = EntityState.Modified;
			}
			await _context.SaveChangesAsync();
			return RedirectToPage("Details", new { id = id });
		}
		public async Task<IActionResult> OnPostPostear(int id)
		{
			var archivo = HttpContext.Request.Form.Files.FirstOrDefault();

			if (archivo != null)
			{
				imagen = new Imagenes();
				using (var bReader = new BinaryReader(archivo.OpenReadStream()))
				{
					imagen.Imagen = bReader.ReadBytes((int)archivo.Length);
					imagen.ImagenNombre = archivo.Name;
				}
			}

			Publicacion.ForoId = id;
			Publicacion.Usuario = User.Identity.Name.Split('-')[0] + " " + User.Identity.Name.Split('-')[1];
			Publicacion.Imagen = imagen;
			Publicacion.Fecha = DateTime.Now;
			_context.Publicacion.Add(Publicacion);
			await _context.SaveChangesAsync();
			return RedirectToPage("Details", new { id = id });
		}
		public async Task<IActionResult> OnPostAprobar(int id)
		{
			Foro.Status = ForumStatus.Aprobado;
			await _context.SaveChangesAsync();
			return RedirectToPage("Details", new { id = id });
		}
		public async Task<IActionResult> OnPostRechazar(int id)
		{
			Foro.Status = ForumStatus.Rechazado;
			await _context.SaveChangesAsync();
			return RedirectToPage("Details", new { id = id });
		}

	}
}