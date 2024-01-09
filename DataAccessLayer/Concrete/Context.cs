using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=HASANALP\\HASANALP;database=AnketDb;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" }
            );

            string yeniSifre = "Admin2020!";
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
            string hashedSifre = passwordHasher.HashPassword(null, yeniSifre);

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = "adminId",
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    NormalizedUserName = "ADMIN@ADMIN.COM",
                    PasswordHash =hashedSifre
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId = "adminId" }
            );

            modelBuilder.Entity<Anket>().ToTable("Anketler");
            modelBuilder.Entity<Soru>().ToTable("Sorular");
            modelBuilder.Entity<Cevap>().ToTable("Cevaplar");

            modelBuilder.Entity<Cevap>()
                .HasOne(c => c.Soru)
                .WithMany(s => s.Cevaplar)
                .HasForeignKey(c => c.SoruId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Anket>()
                .HasMany(a => a.Sorular)
                .WithOne(s => s.Anket)
                .HasForeignKey(s => s.AnketId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Soru>()
                .HasMany(s => s.Cevaplar)
                .WithOne(c => c.Soru)
                .HasForeignKey(c => c.SoruId)
                .OnDelete(DeleteBehavior.Cascade);

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().HasData(
               new AppUser { Id = "user1", UserName = "user1@hotmail.com", Email = "user1@hotmail.com", NormalizedEmail = "USER1@HOTMAIL.COM", NormalizedUserName = "USER1@HOTMAIL.COM", PasswordHash = "QW1?45dx_wq^=" }
           );
            modelBuilder.Entity<Anket>().HasData(
               new Anket { Id = 1, Baslik = "Vücut Yağ Ölçüm Anketi" },
               new Anket { Id = 2, Baslik = "Ne Kadar Mutlusunuz Anketi" }
           );

            modelBuilder.Entity<Soru>().HasData(
    new Soru { Id = 1, Metin = "Kaç Kilosunuz ?", AnketId = 1 },
    new Soru { Id = 2, Metin = "Cinsiyetiniz Nedir ?", AnketId = 1 },
    new Soru { Id = 3, Metin = "Haftada kaç saat kendinize vakit ayırıyorsunuz ? ", AnketId = 2 }
);
            modelBuilder.Entity<Cevap>().HasData(
    new Cevap { Id = 1, CevapMetni = "78", UserId = "user1", AnketId = 1, SoruId = 1 }
);

        }
    }
}
