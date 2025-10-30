using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Data;

public class PDVContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Command> Commands { get; set; }
    public DbSet<CommandItem> CommandItems { get; set; }
    public DbSet<Commission> Commissions { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Subgroup> Subgroups { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=pdv.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(e => e.Group)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.Subgroup)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.SubgroupId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Command>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CommandNumber).IsUnique();
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.HasMany(e => e.Items)
                .WithOne(e => e.Command)
                .HasForeignKey(e => e.CommandId);
        });

        modelBuilder.Entity<CommandItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Commission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasMany(e => e.Subgroups)
                .WithOne(e => e.Group)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Subgroup>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name);
        });

        // Seed initial data
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Code = 2, Name = "Cerveja", Price = 5.00M, Category = "Bebida" },
            new Product { Id = 2, Code = 52, Name = "Balde de Skol", Price = 25.00M, Category = "Bebida" },
            new Product { Id = 3, Code = 50, Name = "Comissão", Price = 50.00M, Category = "Serviço" }
        );
    }
}
