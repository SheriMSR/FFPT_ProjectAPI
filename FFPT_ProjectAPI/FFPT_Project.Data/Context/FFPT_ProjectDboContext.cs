using System;
using System.Collections.Generic;
using FFPT_Project.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FFPT_Project.Data.Context
{
    public partial class FFPT_ProjectDboContext : DbContext
    {
        public FFPT_ProjectDboContext()
        {
        }

        public FFPT_ProjectDboContext(DbContextOptions<FFPT_ProjectDboContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<ComboProduct> ComboProducts { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Floor> Floors { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductInMenu> ProductInMenus { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:ffpt-project.database.windows.net,1433;Initial Catalog=FFPT_ProjectDbo;Persist Security Info=False;User ID=administrators;Password=zaQ@1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CategoryName).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<ComboProduct>(entity =>
            {
                entity.ToTable("ComboProduct");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Discount)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ComboProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ComboProduct_Product");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.ImageUrl).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.ToTable("Floor");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FloorNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.MenuName).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.TimeSlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Menu_TimeSlot");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CheckInDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryPhone).HasMaxLength(50);

                entity.Property(e => e.OrderName).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Room1");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Shipper");

                entity.HasOne(d => d.SupplierStore)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.SupplierStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Store");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ProductName)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.ProductInMenu)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductInMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_ProductInMenu");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Detail).HasMaxLength(100);

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Category");

                entity.HasOne(d => d.SupplierStore)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierStoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Store");
            });

            modelBuilder.Entity<ProductInMenu>(entity =>
            {
                entity.ToTable("ProductInMenu");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductInMenu_Menu");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductInMenu_Product");

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.ProductInMenus)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductInMenu_Store");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RoomNumber).HasMaxLength(50);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.AreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Area");

                entity.HasOne(d => d.Floor)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.FloorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Room_Floor");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shipper");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.ShipperName).HasMaxLength(50);
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("Store");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreateAt).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.StoreCode).HasMaxLength(50);

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.ToTable("TimeSlot");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
