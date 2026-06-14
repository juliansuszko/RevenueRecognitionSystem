using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class DatabaseContext(DbContextOptions<DatabaseContext> opt, IConfiguration configuration) : DbContext(opt)
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<Token> Tokens { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<IndividualClient> IndividualClients { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<SoftwareCategory> SoftwareCategories { get; set; }
    public virtual DbSet<Software> Softwares { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<ContractStatus> ContractStatuses { get; set; }
    public virtual DbSet<Contract> Contracts { get; set; }
    public virtual DbSet<Subscription> Subscriptions { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema(configuration["DB:Schema"]);

        modelBuilder.Entity<User>(user =>
        {
            user.HasKey(e => e.UserId);
            user.Property(e => e.UserId).ValueGeneratedOnAdd();
            user.Property(e => e.Login).HasMaxLength(256);
            user.Property(e => e.PasswordHash).HasMaxLength(256);
            user.HasIndex(e => e.Login).IsUnique();

            user.HasOne(e => e.UserRole)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);

            user.HasOne(e => e.Token)
                .WithOne(e => e.User)
                .HasForeignKey<Token>(e => e.UserId);
        });

        modelBuilder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(e => e.RoleId);
            userRole.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Token>(token =>
        {
            token.HasKey(e => e.UserId);
            token.Property(e => e.RefreshToken).HasMaxLength(256);
            token.HasIndex(e => e.RefreshToken).IsUnique();
        });

        modelBuilder.Entity<UserRole>().HasData([
            new UserRole
            {
                RoleId = 1,
                Name = "Admin",
            },
            new UserRole
            {
                RoleId = 2,
                Name = "User"
            }
        ]);
        
        modelBuilder.Entity<Client>().UseTptMappingStrategy();

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(c => c.ClientId);
            entity.Property(c => c.Address).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(50);
            entity.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(15);
            entity.Property(c => c.IsDeleted).HasColumnType("bit").HasDefaultValue(false);
        });

        modelBuilder.Entity<IndividualClient>(entity =>
        {
            entity.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Pesel).IsRequired().HasColumnType("char(11)");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(c => c.Name).IsRequired().HasMaxLength(150);
            entity.Property(c => c.Krs).IsRequired().HasColumnType("char(10)");
        });
        
        modelBuilder.Entity<SoftwareCategory>(entity =>
        {
            entity.HasKey(sc => sc.CategoryId);
            entity.Property(sc => sc.Name).IsRequired().HasMaxLength(50);
        });

        modelBuilder.Entity<Software>(entity =>
        {
            entity.HasKey(s => s.SoftwareId);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(150);
            entity.Property(s => s.Description).HasMaxLength(400).IsRequired(false);
            entity.Property(s => s.LatestVersion).IsRequired().HasMaxLength(30);
            entity.Property(s => s.BasePrice).HasColumnType("decimal(18,2)");

            entity.HasOne(s => s.Category)
                .WithMany(sc => sc.Softwares)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(d => d.DiscountId);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(150);
            entity.Property(d => d.Value).HasColumnType("decimal(5,2)").IsRequired();

            entity.HasOne(d => d.Software)
                .WithMany(s => s.Discounts)
                .HasForeignKey(d => d.SoftwareId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ContractStatus>(entity =>
        {
            entity.HasKey(cs => cs.StatusId);
            entity.Property(cs => cs.StatusName).IsRequired().HasMaxLength(150);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(c => c.ContractId);
            entity.Property(c => c.SoftwareVersion).IsRequired().HasMaxLength(30);
            entity.Property(c => c.Price).HasColumnType("decimal(20,2)").IsRequired();

            entity.HasOne(c => c.Client)
                .WithMany(cl => cl.Contracts)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Software)
                .WithMany(s => s.Contracts)
                .HasForeignKey(c => c.SoftwareId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Status)
                .WithMany(cs => cs.Contracts)
                .HasForeignKey(c => c.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(s => s.SubscriptionId);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(150);
            entity.Property(s => s.Price).HasColumnType("decimal(20,2)").IsRequired();

            entity.HasOne(s => s.Client)
                .WithMany(cl => cl.Subscriptions)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Software)
                .WithMany(sw => sw.Subscriptions)
                .HasForeignKey(s => s.SoftwareId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.PaymentId);
            entity.Property(p => p.Amount).HasColumnType("decimal(20,2)").IsRequired();

            entity.HasOne(p => p.Client)
                .WithMany(cl => cl.Payments)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Contract)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.ContractId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            entity.HasOne(p => p.Subscription)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });
    }
}