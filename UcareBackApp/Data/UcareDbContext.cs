using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UcareBackApp.Cards.Entities;
using UcareBackApp.Chats.Entities;
using UcareBackApp.Identity.Entities;

namespace UcareBackApp.Data
{
    public class UcareDbContext : IdentityDbContext<UcareUser, UcareRole, Guid>
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<Chat> Chats { get; set; }

        public UcareDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<UcareUser>(entity =>
                {
                    entity.HasOne<Card>()
                        .WithOne()
                        .HasForeignKey<Card>(c => c.UserId)
                        .OnDelete(DeleteBehavior.Cascade);

                    entity.Property(c => c.Id)
                        .HasDefaultValueSql("gen_random_uuid()"); 
                });

               modelBuilder.Entity<Chat>(entity =>
               {
                   entity.HasOne<Card>()
                        .WithMany()
                        .HasForeignKey(c => c.CardId)
                        .OnDelete(DeleteBehavior.SetNull);

                    entity.Property(c => c.Messages)
                        .HasColumnType("jsonb");

                    entity.Property(c => c.Id)
                        .HasDefaultValueSql("gen_random_uuid()"); 
               });
        }
    }
}