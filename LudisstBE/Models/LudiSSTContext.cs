using LudiSST.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace LudiSST.Data
{
    public class LudiSSTContext : DbContext
    {
        public LudiSSTContext(DbContextOptions<LudiSSTContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Documento)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.Nombre)
                .IsUnique();
        }
    }
}
