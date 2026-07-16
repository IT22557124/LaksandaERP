using Laksanda.API.Domain.Entities;
using Laksanda.API.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    public DbSet<GoodsReceivedNote> GoodsReceivedNotes => Set<GoodsReceivedNote>();

    public DbSet<GoodsReceivedNoteItem> GoodsReceivedNoteItems => Set<GoodsReceivedNoteItem>();

    public DbSet<RawMaterial> RawMaterials => Set<RawMaterial>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(x => x.PurchaseOrderId);

            entity.Property(x => x.PONumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.OrderDate)
                .HasColumnType("timestamp without time zone");

            entity.HasOne(x => x.Supplier)
                .WithMany()
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.PONumber)
                .IsUnique();
        });

        builder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasKey(x => x.PurchaseOrderItemId);

            entity.Property(x => x.Quantity)
                .HasPrecision(18, 2);

            entity.Property(x => x.Price)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.RawMaterial)
                .WithMany()
                .HasForeignKey(x => x.RawMaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<GoodsReceivedNote>(entity =>
        {
            entity.HasKey(x => x.GoodsReceivedNoteId);

            entity.Property(x => x.GRNNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.ReceivedDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(x => x.Remarks)
                .HasMaxLength(500);

            entity.HasOne(x => x.PurchaseOrder)
                .WithMany()
                .HasForeignKey(x => x.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => x.GRNNumber)
                .IsUnique();
        });

        builder.Entity<GoodsReceivedNoteItem>(entity =>
        {
            entity.HasKey(x => x.GoodsReceivedNoteItemId);

            entity.Property(x => x.ReceivedQuantity)
                .HasPrecision(18, 2);

            entity.Property(x => x.UnitCost)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.GoodsReceivedNote)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.GoodsReceivedNoteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.RawMaterial)
                .WithMany()
                .HasForeignKey(x => x.RawMaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<RawMaterial>(entity =>
        {
            entity.HasKey(x => x.RawMaterialId);

            entity.Property(x => x.MaterialCode)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.MaterialName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Unit)
                .HasMaxLength(50);

            entity.Property(x => x.CurrentStock)
                .HasPrecision(18, 2);

            entity.Property(x => x.ReorderLevel)
                .HasPrecision(18, 2);

            entity.Property(x => x.Cost)
                .HasPrecision(18, 2);

            entity.HasIndex(x => x.MaterialCode)
                .IsUnique();
        });

        builder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.ProductId);

            entity.Property(x => x.ProductCode)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.ProductName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Category)
                .HasMaxLength(100);

            entity.Property(x => x.Unit)
                .HasMaxLength(50);

            entity.Property(x => x.SellingPrice)
                .HasPrecision(18, 2);

            entity.Property(x => x.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(x => x.ProductCode)
                .IsUnique();

            entity.HasIndex(x => x.Barcode)
                .IsUnique();
        });

        builder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.CustomerId);

            entity.Property(x => x.CustomerCode)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Phone)
                .HasMaxLength(30);

            entity.Property(x => x.Address)
                .HasMaxLength(500);

            entity.Property(x => x.Email)
                .HasMaxLength(254);

            entity.Property(x => x.CreditLimit)
                .HasPrecision(18, 2);

            entity.HasIndex(x => x.CustomerCode)
                .IsUnique();

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });

        builder.Entity<Supplier>(entity =>
        {
            entity.HasKey(x => x.SupplierId);

            entity.Property(x => x.SupplierCode)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(x => x.SupplierName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Phone)
                .HasMaxLength(30);

            entity.Property(x => x.Email)
                .HasMaxLength(254);

            entity.Property(x => x.Address)
                .HasMaxLength(500);

            entity.HasIndex(x => x.SupplierCode)
                .IsUnique();

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });
    }
}