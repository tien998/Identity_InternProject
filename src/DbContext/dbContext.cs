using Microsoft.EntityFrameworkCore;
using Model.AuthenModels;

public class AuthenDb : DbContext
{
    public AuthenDb(DbContextOptions<AuthenDb> options)
        : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Permission> Permission { get; set; }
    public DbSet<Role_User> Role_User { get; set; }
    public DbSet<Role_Permission> Role_Permission { get; set; }
    public DbSet<User_language> User_language { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User_Language).WithMany(e => e.User).HasForeignKey(e => e.Language_Id);
            entity.HasIndex(e => e.UserName).IsUnique();
        });

        modelBuilder.Entity<Role>().HasKey(e => e.Id);
        modelBuilder.Entity<Permission>().HasKey(e => e.Id);
        modelBuilder.Entity<User_language>().HasKey(e => e.Id);

        modelBuilder.Entity<Role_User>(entity =>
        {
            entity.HasKey(e => new { e.User_Id, e.Role_Id, });
            entity.HasOne(e => e.User).WithMany(e => e.Role_User).HasForeignKey(e => e.User_Id);
            entity.HasOne(e => e.Role).WithMany(e => e.Role_User).HasForeignKey(e => e.Role_Id);
        });

        modelBuilder.Entity<Role_Permission>(entity =>
        {
            entity.HasKey(e => new { e.Role_Id, e.Permission_Id });
            entity.HasOne(e => e.Role).WithMany(e => e.Role_Permissions).HasForeignKey(e => e.Role_Id);
            entity.HasOne(e => e.Permission).WithMany(e => e.Role_Permissions).HasForeignKey(e => e.Permission_Id);
        });
    }
}