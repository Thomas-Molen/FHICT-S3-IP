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

        public virtual DbSet<Users> Users { get; set; }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Email).HasColumnName("email")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(model => model.Username).HasColumnName("username")
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(model => model.Admin).HasColumnName("admin")
                    .HasDefaultValue(0);

                entity.Property(model => model.Password).HasColumnName("password")
                    .HasMaxLength(200)
                    .IsRequired();
            });
            OnModelCreatingPartial(modelBuilder);
        }
    }
}
