using timely_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace timely_backend;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
    public ApplicationDbContext() : base() {
    }

    public DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Classroom> Classrooms { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonName> LessonNames { get; set; }
    public DbSet<LessonTag> LessonTags { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<TimeInterval> TimeIntervals { get; set; }
    public DbSet<Domain> Domains { get; set; }






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
        /*modelBuilder.Entity<Classroom>().HasKey(x => x.Id);
        modelBuilder.Entity<Group>().HasKey(x => x.Id);*/
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
        optionsBuilder.UseMySql(configuration.GetConnectionString("MySQLDatabase"), new MySqlServerVersion(new Version(8, 0, 31)));
    }
}