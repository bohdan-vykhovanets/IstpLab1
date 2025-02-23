using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RestaurantInfrastructure;

namespace RestaurantInfrastructure.Context;

public partial class RestaurantDbContext : DbContext
{
    public RestaurantDbContext()
    {
    }

    public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Cousine> Cousines { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<PreOrder> PreOrders { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=RestaurantDb;Username=postgres;Password=685453");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("Category_pkey");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "Category_Name_key").IsUnique();

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Cousine>(entity =>
        {
            entity.HasKey(e => e.CousineId).HasName("Cousine_pkey");

            entity.ToTable("Cousine");

            entity.HasIndex(e => e.Name, "Cousine_Name_key").IsUnique();

            entity.Property(e => e.CousineId).HasColumnName("CousineID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.MenuItemId).HasName("MenuItem_pkey");

            entity.ToTable("MenuItem");

            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Price).HasPrecision(10, 2);

            entity.HasMany(d => d.Categories).WithMany(p => p.MenuItems)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuItemCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MenuItemCategory_CategoryID_fkey"),
                    l => l.HasOne<MenuItem>().WithMany()
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MenuItemCategory_MenuItemID_fkey"),
                    j =>
                    {
                        j.HasKey("MenuItemId", "CategoryId").HasName("MenuItemCategory_pkey");
                        j.ToTable("MenuItemCategory");
                        j.HasIndex(new[] { "MenuItemId", "CategoryId" }, "MenuItemCategory_MenuItemID_CategoryID_idx").IsUnique();
                        j.IndexerProperty<int>("MenuItemId").HasColumnName("MenuItemID");
                        j.IndexerProperty<int>("CategoryId").HasColumnName("CategoryID");
                    });

            entity.HasMany(d => d.Cousines).WithMany(p => p.MenuItems)
                .UsingEntity<Dictionary<string, object>>(
                    "MenuItemCousine",
                    r => r.HasOne<Cousine>().WithMany()
                        .HasForeignKey("CousineId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MenuItemCousine_CousineID_fkey"),
                    l => l.HasOne<MenuItem>().WithMany()
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("MenuItemCousine_MenuItemID_fkey"),
                    j =>
                    {
                        j.HasKey("MenuItemId", "CousineId").HasName("MenuItemCousine_pkey");
                        j.ToTable("MenuItemCousine");
                        j.HasIndex(new[] { "MenuItemId", "CousineId" }, "MenuItemCousine_MenuItemID_CousineID_idx").IsUnique();
                        j.IndexerProperty<int>("MenuItemId").HasColumnName("MenuItemID");
                        j.IndexerProperty<int>("CousineId").HasColumnName("CousineID");
                    });
        });

        modelBuilder.Entity<PreOrder>(entity =>
        {
            entity.HasKey(e => e.PreOrderId).HasName("PreOrder_pkey");

            entity.ToTable("PreOrder");

            entity.Property(e => e.PreOrderId).HasColumnName("PreOrderID");
            entity.Property(e => e.MenuItemId).HasColumnName("MenuItemID");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.PreOrders)
                .HasForeignKey(d => d.MenuItemId)
                .HasConstraintName("PreOrder_MenuItemID_fkey");

            entity.HasOne(d => d.Reservation).WithMany(p => p.PreOrders)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("PreOrder_ReservationID_fkey");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.ReservationId).HasName("Reservation_pkey");

            entity.ToTable("Reservation");

            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");
            entity.Property(e => e.ReservationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Reservation_UserID_fkey");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("Review_pkey");

            entity.ToTable("Review");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Review_UserID_fkey");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("Table_pkey");

            entity.ToTable("Table");

            entity.Property(e => e.TableId).HasColumnName("TableID");
            entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Tables)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("Table_ReservationID_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "User_Email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
