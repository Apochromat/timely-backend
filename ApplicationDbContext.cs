using timely_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace timely_backend;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
    private readonly IConfiguration _configuration;
    public ApplicationDbContext(IConfiguration configuration) : base() {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(e => {
            e.ToTable("Users");
        });
        modelBuilder.Entity<Role>(e => {
            e.ToTable("Roles");
        });
        modelBuilder.Entity<UserRole>(e => {
            e.ToTable("UserRoles");
            e.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.User)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseMySql(_configuration.GetConnectionString("MySQLDatabase"), new MySqlServerVersion(new Version(8, 0, 31)));
    }
}