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

    public virtual DbSet<Authentification> Authentifications { get; set; }

    public virtual DbSet<Box> Boxes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<PurchaseBox> PurchaseBoxes { get; set; }

    public virtual DbSet<PurchaseProduct> PurchaseProducts { get; set; }

    public virtual DbSet<PurchaseWaste> PurchaseWastes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripBox> TripBoxes { get; set; }

    public virtual DbSet<TripCharge> TripCharges { get; set; }

    public virtual DbSet<TripProduct> TripProducts { get; set; }

    public virtual DbSet<TripWaste> TripWastes { get; set; }

    public virtual DbSet<Truck> Trucks { get; set; }

    public virtual DbSet<Waste> Wastes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authentification>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("PK_Auth");

            entity.ToTable("Authentification");

            entity.Property(e => e.Login)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password).IsUnicode(false);
        });

        modelBuilder.Entity<Box>(entity =>
        {
            entity.ToTable("Box");

            entity.Property(e => e.Id)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ID");
            entity.Property(e => e.Designation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Cin);

            entity.ToTable("Employee");

            entity.Property(e => e.Cin)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Commission).HasColumnType("money");
            entity.Property(e => e.Name)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.Salary).HasColumnType("money");
            entity.Property(e => e.Tel)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => new { e.Employee, e.Month, e.Year }).HasName("PK_PEmp");

            entity.ToTable("Payment");

            entity.Property(e => e.Employee)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Commission).HasColumnType("money");
            entity.Property(e => e.Date).HasColumnType("date");
            entity.Property(e => e.Salary).HasColumnType("money");

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
            entity.Property(e => e.CommissionDriver).HasColumnType("money");
            entity.Property(e => e.CommissionSeller).HasColumnType("money");
            entity.Property(e => e.Designation).IsUnicode(false);
            entity.Property(e => e.Genre)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.PriceProfessional).HasColumnType("money");
            entity.Property(e => e.Stock).HasColumnName("stock");
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

        modelBuilder.Entity<PurchaseBox>(entity =>
        {
            entity.HasKey(e => new { e.Purchase, e.Box });

            entity.ToTable("PurchaseBox");

            entity.Property(e => e.Purchase)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Box)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.BoxNavigation).WithMany(p => p.PurchaseBoxes)
                .HasForeignKey(d => d.Box)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PBBox");

            entity.HasOne(d => d.PurchaseNavigation).WithMany(p => p.PurchaseBoxes)
                .HasForeignKey(d => d.Purchase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PBPurchase");
        });

        modelBuilder.Entity<PurchaseProduct>(entity =>
        {
            entity.HasKey(e => new { e.Purchase, e.Product }).HasName("PK_PurchaseInfo");

            entity.ToTable("PurchaseProduct");

            entity.Property(e => e.Purchase)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.PurchaseProducts)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PPProduct");

            entity.HasOne(d => d.PurchaseNavigation).WithMany(p => p.PurchaseProducts)
                .HasForeignKey(d => d.Purchase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PPPurchase");
        });

        modelBuilder.Entity<PurchaseWaste>(entity =>
        {
            entity.HasKey(e => new { e.Purchase, e.Product, e.Type });

            entity.ToTable("PurchaseWaste");

            entity.Property(e => e.Purchase)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.PurchaseNavigation).WithMany(p => p.PurchaseWastes)
                .HasForeignKey(d => d.Purchase)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PWPurchase");

            entity.HasOne(d => d.Waste).WithMany(p => p.PurchaseWastes)
                .HasForeignKey(d => new { d.Product, d.Type })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PWProduct");
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
            entity.ToTable("Trip");

            entity.Property(e => e.Id)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ID");
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
                .HasConstraintName("FK_TDriver");

            entity.HasOne(d => d.HelperNavigation).WithMany(p => p.TripHelperNavigations)
                .HasForeignKey(d => d.Helper)
                .HasConstraintName("FK_THelper");

            entity.HasOne(d => d.SellerNavigation).WithMany(p => p.TripSellerNavigations)
                .HasForeignKey(d => d.Seller)
                .HasConstraintName("FK_TSeller");

            entity.HasOne(d => d.TruckNavigation).WithMany(p => p.Trips)
                .HasForeignKey(d => d.Truck)
                .HasConstraintName("FK_TTruck");
        });

        modelBuilder.Entity<TripBox>(entity =>
        {
            entity.HasKey(e => new { e.Trip, e.Box });

            entity.ToTable("TripBox");

            entity.Property(e => e.Trip)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Box)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.BoxNavigation).WithMany(p => p.TripBoxes)
                .HasForeignKey(d => d.Box)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TBProd");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripBoxes)
                .HasForeignKey(d => d.Trip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TBTrip");
        });

        modelBuilder.Entity<TripCharge>(entity =>
        {
            entity.HasKey(e => new { e.Trip, e.Type }).HasName("PK_TCharge");

            entity.Property(e => e.Trip)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.Amount).HasColumnType("money");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripCharges)
                .HasForeignKey(d => d.Trip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TCTrip");
        });

        modelBuilder.Entity<TripProduct>(entity =>
        {
            entity.HasKey(e => new { e.Trip, e.Product });

            entity.ToTable("TripProduct");

            entity.Property(e => e.Trip)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("money");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.TripProducts)
                .HasForeignKey(d => d.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TPProd");

            entity.HasOne(d => d.TripNavigation).WithMany(p => p.TripProducts)
                .HasForeignKey(d => d.Trip)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TPTrip");
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
