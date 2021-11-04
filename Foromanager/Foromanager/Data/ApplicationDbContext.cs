using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Foromanager.Models;

namespace Foromanager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Foro>()
                 .HasMany(f => f.Categorias)
                 .WithMany(c => c.Foros)
                 .UsingEntity(j => j.ToTable("ForoCategoria"));
        }

        public DbSet<Foromanager.Models.Foro> Foro { get; set; }
        public DbSet<Foromanager.Models.Publicacion> Publicacion { get; set; }
        public DbSet<Foromanager.Models.Categoria> Categoria { get; set; }
        public DbSet<Foromanager.Models.Reaccion> Reaccion { get; set; }
    }
}
