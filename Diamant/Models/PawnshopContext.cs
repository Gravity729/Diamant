using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Diamant.Models;

public partial class PawnshopContext : DbContext
{
    public PawnshopContext()
    {
    }

    public PawnshopContext(DbContextOptions<PawnshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ReturnedItem> ReturnedItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-J3NOIFB9\\SQLEXPRESS01;Database=Pawnshop;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Client__81A2CB81AABB33BC");

            entity.ToTable("Client");

            entity.HasIndex(e => e.Phone, "UQ__Client__B43B145F77537FFA").IsUnique();

            entity.Property(e => e.ClientId).HasColumnName("clientID");
            entity.Property(e => e.BDate).HasColumnName("bDate");
            entity.Property(e => e.FName)
                .HasMaxLength(50)
                .HasColumnName("fName");
            entity.Property(e => e.LName)
                .HasMaxLength(50)
                .HasColumnName("lName");
            entity.Property(e => e.NPassport)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("n_passport");
            entity.Property(e => e.PName)
                .HasMaxLength(50)
                .HasColumnName("pName");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.SPassport)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("s_passport");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__C134C9A1327BE939");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.LoginE, "UQ__Employee__94F8E953370D0508").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Employee__B43B145FEB6C4C96").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("employeeID");
            entity.Property(e => e.BDate).HasColumnName("bDate");
            entity.Property(e => e.FName)
                .HasMaxLength(50)
                .HasColumnName("fName");
            entity.Property(e => e.LName)
                .HasMaxLength(50)
                .HasColumnName("lName");
            entity.Property(e => e.LoginE)
                .HasMaxLength(50)
                .HasColumnName("loginE");
            entity.Property(e => e.NPassport)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("n_passport");
            entity.Property(e => e.PName)
                .HasMaxLength(50)
                .HasColumnName("pName");
            entity.Property(e => e.PasswordE)
                .HasMaxLength(50)
                .HasColumnName("passwordE");
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.SPassport)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("s_passport");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D14A31DF60E4");

            entity.ToTable("Product", tb => tb.HasTrigger("delProduct"));

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.AssessedValue)
                .HasColumnType("money")
                .HasColumnName("assessedValue");
            entity.Property(e => e.BailAmount)
                .HasColumnType("money")
                .HasColumnName("bailAmount");
            entity.Property(e => e.ClientId).HasColumnName("clientID");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeID");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(50)
                .HasColumnName("nameProduct");
            entity.Property(e => e.ShelfLife).HasColumnName("shelfLife");
            entity.Property(e => e.StatusProduct)
                .HasMaxLength(10)
                .HasColumnName("statusProduct");

            entity.HasOne(d => d.Client).WithMany(p => p.Products)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Client");

            entity.HasOne(d => d.Employee).WithMany(p => p.Products)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Employee");
        });

        modelBuilder.Entity<ReturnedItem>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.AssessedValue)
                .HasColumnType("money")
                .HasColumnName("assessedValue");
            entity.Property(e => e.BailAmount)
                .HasColumnType("money")
                .HasColumnName("bailAmount");
            entity.Property(e => e.ClientId).HasColumnName("clientID");
            entity.Property(e => e.DeletionDate)
                .HasColumnType("datetime")
                .HasColumnName("deletionDate");
            entity.Property(e => e.DueDate).HasColumnName("dueDate");
            entity.Property(e => e.EmployeeId).HasColumnName("employeeID");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(50)
                .HasColumnName("nameProduct");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.ShelfLife).HasColumnName("shelfLife");
            entity.Property(e => e.StatusProduct)
                .HasMaxLength(10)
                .HasColumnName("statusProduct");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}