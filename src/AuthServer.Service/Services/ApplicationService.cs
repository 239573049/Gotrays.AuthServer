namespace AuthServer.Service.Services;

public abstract class ApplicationService<T> : ServiceBase where T : class
{
    public IEventBus EventBus => GetRequiredService<IEventBus>();
    
    public ILogger<T> Logger => GetRequiredService<ILogger<T>>();
    
}