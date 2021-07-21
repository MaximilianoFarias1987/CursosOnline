using Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia
{
    public class CursosContext : DbContext
    {
        public CursosContext(DbContextOptions options) : base(options)
        {

        }

        //Indicamos que tenemos una entidad con una clave primaria compuesta, para esto necesitamos agregar el using Dominio.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new { ci.InstructorId, ci.CursoId });
        }

        //Ahora conviero en entidades cada una de las clases que estan en dominio.

        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoInstructor> CursoInstructores { get; set; }
        public DbSet<Instructor> Instructores { get; set; }
        public DbSet<Precio> Precios { get; set; }
    }
}
