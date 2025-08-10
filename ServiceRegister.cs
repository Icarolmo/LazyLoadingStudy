using Microsoft.EntityFrameworkCore;

public static class ServiceRegister
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
    {
        services.AddDbContext<MyDbContext>(options =>
            options
                .UseLazyLoadingProxies()
                .UseMySql("server=localhost;database=testing;user=root;password=root123",
                new MySqlServerVersion(new Version(8, 0, 21))));

        return services;
    }
}   

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.username).HasColumnName("username");
            entity.Property(e => e.email).HasColumnName("email");

            entity.HasMany(e => e.posts).WithOne(e => e.user).HasForeignKey(e => e.user_id);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("posts");
            entity.HasKey(e => e.id);
            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.message).HasColumnName("message");
            entity.Property(e => e.user_id).HasColumnName("user_id");
        });
    }
}

public class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public virtual ICollection<Post> posts { get; set; }
}

public class Post
{
    public int id { get; set; }
    public string message { get; set; }
    public int user_id { get; set; }
    public virtual User user { get; set; }
}

