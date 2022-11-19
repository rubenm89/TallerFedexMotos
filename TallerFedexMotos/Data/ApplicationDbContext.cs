using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TallerFedexMotos.Models;

namespace TallerFedexMotos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> clientes { get; set; }
        public DbSet<Marca> marcas { get; set; }
        public DbSet<Modelo> modelos { get; set; }
        public DbSet<Moto> motos { get; set; }
        public DbSet<MotoEnReparacion> motosEnReparacion { get; set; }
        public DbSet<MotoEnVenta> motosEnVenta { get; set; }
    }
}
