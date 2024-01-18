namespace AuthServer.Service.DataAccess;

public class AuthDbContext(MasaDbContextOptions<AuthDbContext> options) : MasaDbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreatingExecuting(ModelBuilder modelBuilder)
    {
        base.OnModelCreatingExecuting(modelBuilder);
        ConfigEntities(modelBuilder);
    }

    private static void ConfigEntities(ModelBuilder builder)
    {
        builder.Entity<User>(options =>
        {
            options.ToTable(Constant.DefaultTablePrefix + "Users", Constant.DefaultSchema);
            
            options.HasKey(x => x.Id);
            
            options.Property(x => x.Id).HasColumnName("Id").IsRequired();
            
            options.Property(x => x.UserName).HasColumnName("UserName").IsRequired();
            
        });

    }
}