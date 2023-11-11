using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JaoudaMS_API.Models;

public partial class JaoudaSmContext : DbContext
{
    public JaoudaSmContext()
    {
    }

    public JaoudaSmContext(DbContextOptions<JaoudaSmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<InBox> InBoxes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseInfo> PurchaseInfos { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripInfo> TripInfos { get; set; }

    public virtual DbSet<TripWaste> TripWastes { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<Waste> Wastes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Box>(entity =>
        {
            entity.HasKey(e => e.Type);

            entity.ToTable("Box");

            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SubBox)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SubBoxNavigation).WithMany(p => p.InverseSubBoxNavigation)
                .HasForeignKey(d => d.SubBox)
                .HasConstraintName("FK_BBox");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Cin);

            entity.ToTable("Employee");

            entity.Property(e => e.Cin)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Tel)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InBox>(entity =>
        {
            entity.HasKey(e => new { e.Product, e.Box, e.Capacity }).HasName("PK_BContain");

            entity.ToTable("InBox");

            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Box)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.BoxNavigation).WithMany(p => p.InBoxes)
                .HasForeignKey(d => d.Box)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InBoxBox");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.InBoxes)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InBoxPrd");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => new { e.Employee, e.Month, e.Year }).HasName("PK_PEmp");

            entity.ToTable("Payment");

            entity.Property(e => e.Employee)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Commission).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEmp");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Designation).IsUnicode(false);
            entity.Property(e => e.Genre).IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchase");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Supplier)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SupplierNavigation).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.Supplier)
                .HasConstraintName("FK_PrchSupplier");
        });

        modelBuilder.Entity<PurchaseInfo>(entity =>
        {
            entity.HasKey(e => new { e.Purchase, e.Product, e.Box });

            entity.ToTable("PurchaseInfo");

            entity.Property(e => e.Purchase)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Box)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");

            entity.HasOne(d => d.BoxNavigation).WithMany(p => p.PurchaseInfos)
                .HasForeignKey(d => d.Box)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PIBox");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.PurchaseInfos)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PIProduct");

            entity.HasOne(d => d.PurchaseNavigation).WithMany(p => p.PurchaseInfos)
                .HasForeignKey(d => d.Purchase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PIPurchase");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Supplier");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Tel)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TripID");

            entity.ToTable("Trip");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Charges).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Driver)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Helper)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Seller)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Truck)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Zone)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.DriverNavigation).WithMany(p => p.TripDriverNavigations)
                .HasForeignKey(d => d.Driver)
                .HasConstraintName("FK_TripDriver");

            entity.HasOne(d => d.HelperNavigation).WithMany(p => p.TripHelperNavigations)
                .HasForeignKey(d => d.Helper)
                .HasConstraintName("FK_TripHelper");

            entity.HasOne(d => d.SellerNavigation).WithMany(p => p.TripSellerNavigations)
                .HasForeignKey(d => d.Seller)
                .HasConstraintName("FK_TripSeller");

            entity.HasOne(d => d.TruckNavigation).WithMany(p => p.Trips)
                .HasForeignKey(d => d.Truck)
                .HasConstraintName("FK_TripTruck");
        });

        modelBuilder.Entity<TripInfo>(entity =>
        {
            entity.HasKey(e => new { e.Trip, e.Product, e.Box }).HasName("PK_TripInf");

            entity.ToTable("TripInfo");

            entity.Property(e => e.Trip)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Box)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.BoxNavigation).WithMany(p => p.TripInfos)
                .HasForeignKey(d => d.Box)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIBox");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.TripInfos)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TIProd");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripInfos)
                .HasForeignKey(d => d.Trip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TITrip");
        });

        modelBuilder.Entity<TripWaste>(entity =>
        {
            entity.HasKey(e => new { e.Trip, e.Product, e.Type }).HasName("PK_TWaste");

            entity.ToTable("TripWaste");

            entity.Property(e => e.Trip)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Qtt)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripWastes)
                .HasForeignKey(d => d.Trip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TWTrip");

            entity.HasOne(d => d.Waste).WithMany(p => p.TripWastes)
                .HasForeignKey(d => new { d.Product, d.Type })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TWWaste");
        });

        modelBuilder.Entity<Truck>(entity =>
        {
            entity.HasKey(e => e.Matricula);

            entity.ToTable("Truck");

            entity.Property(e => e.Matricula)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Waste>(entity =>
        {
            entity.HasKey(e => new { e.Product, e.Type });

            entity.ToTable("Waste");

            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Wastes)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WastePrd");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
