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
        public DbSet<Foromanager.Models.Foro> Foro { get; set; }
        public DbSet<Foromanager.Models.Publicacion> Publicacion {get;set;}
    }
}
