namespace Datos.Modelo
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model11")
        {
        }

        public virtual DbSet<Ambiente> Ambiente { get; set; }
        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Competencia> Competencia { get; set; }
        public virtual DbSet<Coordinacion> Coordinacion { get; set; }
        public virtual DbSet<Departamento> Departamento { get; set; }
        public virtual DbSet<Ficha> Ficha { get; set; }
        public virtual DbSet<Ficha_Ambiente> Ficha_Ambiente { get; set; }
        public virtual DbSet<Instructor> Instructor { get; set; }
        public virtual DbSet<Municipio> Municipio { get; set; }
        public virtual DbSet<Programa> Programa { get; set; }
        public virtual DbSet<Resultado_Aprendizaje> Resultado_Aprendizaje { get; set; }
        public virtual DbSet<Sede> Sede { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Solicitud> Solicitud { get; set; }
        public virtual DbSet<PrestamoLlaves> PrestamoLlaves { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Ambiente>()
            //    .Property(e => e.Numero)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Ambiente>()
            //    .HasMany(e => e.Ficha_Ambiente)
            //    .WithRequired(e => e.Ambiente)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Area>()
            //    .Property(e => e.Nombre)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Area>()
            //    .Property(e => e.Descripcion)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Area>()
            //    .HasMany(e => e.Ambiente)
            //    .WithRequired(e => e.Area)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Area>()
            //    .HasMany(e => e.Programa)
            //    .WithRequired(e => e.Area)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Competencia>()
            //    .Property(e => e.Nombre)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Competencia>()
            //    .HasMany(e => e.Resultado_Aprendizaje)
            //    .WithRequired(e => e.Competencia)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Coordinacion>()
            //    .Property(e => e.Nombre_Coordinacion)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Coordinacion>()
            //    .Property(e => e.Cedula)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Coordinacion>()
            //    .Property(e => e.Nombre)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Coordinacion>()
            //    .Property(e => e.Apellido)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Coordinacion>()
            //    .Property(e => e.Telefono)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Coordinacion>()
            //    .Property(e => e.Correo)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Departamento>()
            //    .Property(e => e.NombreDepartamento)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Departamento>()
            //    .HasMany(e => e.Municipio)
            //    .WithRequired(e => e.Departamento)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Ficha>()
            //    .Property(e => e.Ficha1)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Ficha>()
            //    .Property(e => e.TipoFormacion)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Ficha>()
            //    .Property(e => e.Jornada)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Ficha>()
            //    .HasMany(e => e.Ficha_Ambiente)
            //    .WithRequired(e => e.Ficha)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Ficha_Ambiente>()
            //    .Property(e => e.Color)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Ficha_Ambiente>()
            //    .Property(e => e.NombreEmpresa)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Instructor>()
            //    .Property(e => e.Nombre)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Instructor>()
            //    .Property(e => e.Apellido)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Instructor>()
            //    .Property(e => e.Cedula)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Instructor>()
            //    .Property(e => e.Email)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Instructor>()
            //    .Property(e => e.Telefono)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Instructor>()
            //    .HasMany(e => e.Ficha_Ambiente)
            //    .WithRequired(e => e.Instructor)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Municipio>()
            //    .Property(e => e.IdMunicipio)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Municipio>()
            //    .Property(e => e.NombreMunicipio)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Municipio>()
            //    .HasMany(e => e.Sede)
            //    .WithRequired(e => e.Municipio)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Programa>()
            //    .Property(e => e.Nivel)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Programa>()
            //    .Property(e => e.LineaTecnologica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Programa>()
            //    .Property(e => e.Red_Tecnologica)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Programa>()
            //    .Property(e => e.Perfil_Instructor)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Programa>()
            //    .Property(e => e.NombrePrograma)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Programa>()
            //    .HasMany(e => e.Competencia)
            //    .WithRequired(e => e.Programa)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Programa>()
            //    .HasMany(e => e.Ficha)
            //    .WithRequired(e => e.Programa)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Resultado_Aprendizaje>()
            //    .Property(e => e.Resultado)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Resultado_Aprendizaje>()
            //    .HasMany(e => e.Ficha_Ambiente)
            //    .WithRequired(e => e.Resultado_Aprendizaje)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Sede>()
            //    .Property(e => e.Nombre_Sede)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Sede>()
            //    .Property(e => e.Direccion)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Sede>()
            //    .Property(e => e.IdMunicipio)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Sede>()
            //    .HasMany(e => e.Ambiente)
            //    .WithRequired(e => e.Sede)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.NombreUsuario)
            //    .IsUnicode(false);

            //modelBuilder.Entity<Usuario>()
            //    .Property(e => e.Password)
            //    .IsUnicode(false);
        }
    }
}
