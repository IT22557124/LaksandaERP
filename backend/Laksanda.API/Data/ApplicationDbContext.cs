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

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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