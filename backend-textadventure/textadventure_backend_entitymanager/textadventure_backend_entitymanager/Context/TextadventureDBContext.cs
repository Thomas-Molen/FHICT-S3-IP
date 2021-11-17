using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using textadventure_backend_entitymanager.Models;

namespace textadventure_backend_entitymanager.Context
{
    public partial class TextadventureDBContext : DbContext
    {

        public TextadventureDBContext(DbContextOptions<TextadventureDBContext> options) : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Adventurers> Adventurers { get; set; }
        public virtual DbSet<AdventurerMaps> AdventurerMaps { get; set; }
        public virtual DbSet<Weapons> Weapons { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Dungeons> Dungeons { get; set; }
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<Interactions> Interactions { get; set; }
        public virtual DbSet<NPCs> NPCs { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Email).HasColumnName("email")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(model => model.Username).HasColumnName("username")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(model => model.Admin).HasColumnName("admin")
                    .HasDefaultValue(false);

                entity.Property(model => model.Password).HasColumnName("password")
                    .HasMaxLength(200)
                    .IsRequired();
            });

            modelBuilder.Entity<Adventurers>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Experience).HasColumnName("experience")
                    .IsRequired();

                entity.Property(model => model.Health).HasColumnName("health")
                    .IsRequired();

                entity.Property(model => model.Name).HasColumnName("name")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasOne(a => a.User)
                   .WithMany(u => u.Adventurers)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Adventurers_Users");

                entity.HasOne(a => a.Dungeon)
                   .WithMany(d => d.Adventurers)
                   .HasForeignKey(a => a.DungeonId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Adventurers_Dungeons");

                entity.HasOne(a => a.Room)
                   .WithMany(r => r.Adventurers)
                   .HasForeignKey(a => a.RoomId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Adventurers_Rooms")
                   .IsRequired(false);
            });

            modelBuilder.Entity<AdventurerMaps>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.HasOne(am => am.Adventurer)
                   .WithMany(a => a.AdventurerMaps)
                   .HasForeignKey(am => am.AdventurerId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_AdventurerMaps_Adventurers");

                entity.HasOne(am => am.Room)
                   .WithMany(r => r.AdventurerMaps)
                   .HasForeignKey(am => am.RoomId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_AdventurerMaps_Rooms");
            });

            modelBuilder.Entity<Weapons>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Name).HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(model => model.Attack).HasColumnName("attack")
                    .IsRequired();

                entity.Property(model => model.Durability).HasColumnName("durability")
                    .HasMaxLength(101)
                    .IsRequired();

                entity.Property(model => model.Equiped).HasColumnName("equiped")
                    .IsRequired();

                entity.HasOne(w => w.Adventurer)
                   .WithMany(a => a.Weapons)
                   .HasForeignKey(w => w.AdventurerId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Weapons_Adventurers");
            });

            modelBuilder.Entity<Items>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Name).HasColumnName("name")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(model => model.Description).HasColumnName("description")
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(model => model.Content).HasColumnName("content")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(it => it.Adventurer)
                   .WithMany(a => a.Items)
                   .HasForeignKey(it => it.AdventurerId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Items_Adventurers");
            });

            modelBuilder.Entity<Dungeons>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");
            });

            modelBuilder.Entity<Rooms>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.PositionX).HasColumnName("position_x")
                    .IsRequired();

                entity.Property(model => model.PositionY).HasColumnName("position_y")
                    .IsRequired();

                entity.Property(model => model.Event).HasColumnName("event")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(r => r.Dungeon)
                   .WithMany(d => d.Rooms)
                   .HasForeignKey(r => r.DungeonId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Rooms_Dungeons");

                entity.HasOne(r => r.NorthInteraction)
                   .WithMany(i => i.RoomNorth)
                   .HasForeignKey(r => r.NorthInteractionId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Rooms_NorthInteractions")
                   .IsRequired(false);

                entity.HasOne(r => r.EastInteraction)
                   .WithMany(i => i.RoomEast)
                   .HasForeignKey(r => r.EastInteractionId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Rooms_EastInteractions")
                   .IsRequired(false);

                entity.HasOne(r => r.SouthInteraction)
                   .WithMany(i => i.RoomSouth)
                   .HasForeignKey(r => r.SouthInteractionId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Rooms_SouthInteractions")
                   .IsRequired(false);

                entity.HasOne(r => r.WestInteraction)
                   .WithMany(i => i.RoomWest)
                   .HasForeignKey(r => r.WestInteractionId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Rooms_WestInteractions")
                   .IsRequired(false);
            });

            modelBuilder.Entity<Interactions>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Type).HasColumnName("type")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasOne(i => i.NPC)
                   .WithMany(npc => npc.Interaction)
                   .HasForeignKey(i => i.NPCId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Interactions_NPCs")
                   .IsRequired(false);
            });

            modelBuilder.Entity<NPCs>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Conversation).HasColumnName("conversation")
                    .HasMaxLength(5000)
                    .IsRequired();

                entity.Property(model => model.Risk).HasColumnName("risk")
                    .HasMaxLength(101)
                    .IsRequired();

                entity.HasOne(npc => npc.Weapon)
                   .WithMany(w => w.NPCs)
                   .HasForeignKey(npc => npc.WeaponId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_NPCs_Weapons");

                entity.HasOne(npc => npc.Item)
                   .WithMany(it => it.NPCs)
                   .HasForeignKey(npc => npc.ItemId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_NPCs_Items");
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.Property(model => model.Id).HasColumnName("id");

                entity.Property(model => model.Token).HasColumnName("token")
                    .IsRequired();

                entity.Property(model => model.ExpiresAt).HasColumnName("expires_at")
                    .IsRequired();

                entity.Property(model => model.CreatedAt).HasColumnName("created_at")
                    .IsRequired();

                entity.Property(model => model.RevokedAt).HasColumnName("revoked_at");

                entity.Property(model => model.Active).HasColumnName("active")
                    .IsRequired();

                entity.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_RefreshTokens_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
