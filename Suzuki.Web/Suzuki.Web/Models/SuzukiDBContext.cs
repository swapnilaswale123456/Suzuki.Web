using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Suzuki.Web.Models
{
    public partial class SuzukiDBContext : DbContext
    {
        public SuzukiDBContext()
        {
        }

        public SuzukiDBContext(DbContextOptions<SuzukiDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Distributor> Distributor { get; set; }
        public virtual DbSet<WorkShop> WorkShop { get; set; }
        public virtual DbSet<WorkShopLocation> WorkShopLocation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=VI210920\\SQLEXPRESS;Initial Catalog=SuzukiDB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CountryName).HasMaxLength(500);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Langauge).HasMaxLength(50);
            });

            modelBuilder.Entity<Distributor>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DistributorCode).HasMaxLength(50);

                entity.Property(e => e.DistributorName).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Distributor)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_Distributor_Country");
            });

            modelBuilder.Entity<WorkShop>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.ContactPerson).HasMaxLength(50);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.Property(e => e.WorkShopCode).HasMaxLength(50);

                entity.Property(e => e.WorkShopName).HasMaxLength(500);

                entity.HasOne(d => d.Distributer)
                    .WithMany(p => p.WorkShop)
                    .HasForeignKey(d => d.DistributerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkShop_Distributor");
            });

            modelBuilder.Entity<WorkShopLocation>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address).HasMaxLength(500);

                entity.Property(e => e.ContactPerson).HasMaxLength(50);

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LocationCode).HasMaxLength(50);

                entity.Property(e => e.MobileNumber).HasMaxLength(50);

                entity.HasOne(d => d.WorkShop)
                    .WithMany(p => p.WorkShopLocation)
                    .HasForeignKey(d => d.WorkShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkShopLocation_WorkShop");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
