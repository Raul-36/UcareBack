using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UcareBackApp.Models;

namespace UcareBackApp.Data
{
    public class UcareDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Card> Cards { get; set; }

        public UcareDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}