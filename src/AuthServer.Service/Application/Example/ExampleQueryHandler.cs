namespace AuthServer.Service.Application.Example;

public class ExampleQueryHandler
{
    /// <summary>
    /// This can use query's DbContext
    /// </summary>
    private readonly AuthDbContext _dbContext;

    public ExampleQueryHandler(AuthDbContext dbContext) => _dbContext = dbContext;

    [EventHandler]
    public Task GetListAsync(ExampleGetListQuery command)
    {
        //TODO:Get logic
        return Task.CompletedTask;
    }
}
