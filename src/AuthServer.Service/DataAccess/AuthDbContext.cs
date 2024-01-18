namespace AuthServer.Service.DataAccess;

public class AuthDbContext : MasaDbContext
{
    //public DbSet<UserEntity> { get; set; }

    public AuthDbContext(MasaDbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreatingExecuting(ModelBuilder modelBuilder)
    {
        base.OnModelCreatingExecuting(modelBuilder);
        ConfigEntities(modelBuilder);
    }

    private static void ConfigEntities(ModelBuilder modelBuilder)
    {
        //TODO:Configure Entities.
    }
}
