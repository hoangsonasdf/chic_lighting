using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace chic_lighting.Models
{
    public partial class chic_lightingContext : DbContext
    {
        public chic_lightingContext()
        {
        }

        public chic_lightingContext(DbContextOptions<chic_lightingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartItem> CartItems { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; } = null!;
        public virtual DbSet<Rate> Rates { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Cart_userId_fkey");
            });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CartId).HasColumnName("cartID");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("CartItem_cartID_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("CartItem_productID_fkey");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(25)
                    .HasColumnName("categoryName");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createAt");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("createBy");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifyAt");

                entity.Property(e => e.ModifyBy)
                    .HasMaxLength(50)
                    .HasColumnName("modifyBy");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Comment).HasColumnName("comment");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Rate).HasColumnName("rate");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(50)
                    .HasColumnName("firstname");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("orderDate");

                entity.Property(e => e.OrderStatusId).HasColumnName("orderStatusId");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .HasColumnName("phone");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .HasConstraintName("Order_orderStatusId_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Order_userId_fkey");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("OrderDetail_orderID_fkey");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("OrderDetail_productID_fkey");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Bootstapicon)
                    .HasMaxLength(255)
                    .HasColumnName("bootstapicon");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('payment_id_seq'::regclass)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasIndex(e => e.ProductStatusId, "fki_Product_productStatusId_fkey");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createAt");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("createBy");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Image).HasColumnName("image");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.ModifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("modifyAt");

                entity.Property(e => e.ModifyBy)
                    .HasMaxLength(50)
                    .HasColumnName("modifyBy");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(255)
                    .HasColumnName("productName");

                entity.Property(e => e.ProductStatusId).HasColumnName("productStatusId");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Saleprice).HasColumnName("saleprice");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("Product_categoryId_fkey");

                entity.HasOne(d => d.ProductStatus)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductStatusId)
                    .HasConstraintName("Product_productStatusId_fkey");
            });

            modelBuilder.Entity<ProductStatus>(entity =>
            {
                entity.ToTable("ProductStatus");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('productstatus_id_seq'::regclass)");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Rate>(entity =>
            {
                entity.ToTable("Rate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createAt");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Star).HasColumnName("star");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("Rate_productId_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Rates)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Rate_userId_fkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.HasIndex(e => e.PaymentId, "fki_Transaction_paymentID_fkey");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("orderID");

                entity.Property(e => e.PaymentId).HasColumnName("paymentID");

                entity.Property(e => e.Total).HasColumnName("total");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("Transaction_orderID_fkey");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("Transaction_paymentID_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("createAt");

                entity.Property(e => e.Dob)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.Password)
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(11)
                    .HasColumnName("phone");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");

                entity.Property(e => e.VerifyAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("verifyAt");

                entity.Property(e => e.VerifyCode)
                    .HasMaxLength(6)
                    .HasColumnName("verifyCode");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("User_RoleId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
