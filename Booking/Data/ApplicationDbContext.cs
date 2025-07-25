using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Solution.Models;

namespace Booking.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
   
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Offer> Offers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            
            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(128);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(128);
            
            entity.Property(u => u.DateCreation)
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.HasOne<Role>()
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(128);
            
            entity.HasIndex(r => r.Name).IsUnique();

            entity.Property(r => r.Level)
                .IsRequired();

        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(o => o.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(o => o.Location)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(o => o.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(o => o.BedNumber)
                .IsRequired();

            entity.Property(o => o.BathNumber)
                .IsRequired();

            entity.Property(o => o.NumberOfRooms)
                .IsRequired();

            entity.Property(o => o.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(o => o.Image)
                .HasMaxLength(500);

            // Relation avec User 
            entity.HasOne<User>()
                .WithMany() // Un utilisateur peut avoir plusieurs offres
                .HasForeignKey(o => o.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
            
        });
        
        base.OnModelCreating(modelBuilder);
    }
}
