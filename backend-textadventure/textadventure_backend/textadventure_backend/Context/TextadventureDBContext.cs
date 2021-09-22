using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend.Models;

namespace textadventure_backend.Context
{
    public partial class TextadventureDBContext : DbContext
    {
        public TextadventureDBContext()
        {

        }

        public TextadventureDBContext(DbContextOptions<TextadventureDBContext> options) : base(options)
        {

        }

        public virtual DbSet<users> users { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<users>(entity =>
            {
                entity.Property(model => model.id).HasColumnName("id");

                entity.Property(model => model.email).HasColumnName("email")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(model => model.username).HasColumnName("username")
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(model => model.admin).HasColumnName("admin")
                    .HasDefaultValue(0);

                entity.Property(model => model.password).HasColumnName("password")
                    .HasMaxLength(200)
                    .IsRequired();
            });
            OnModelCreatingPartial(modelBuilder);
        }
    }
}
