namespace AuthServer.Service.Application.Users;

public class UserCommandHandler
{
    /// <summary>
    /// 添加用户
    /// </summary>
    /// <param name="command"></param>
    [EventHandler]
    public async Task CreateUserAsync(CreateUserCommand command)
    {
        
    }
    
    /// <summary>
    /// 删除指定用户
    /// </summary>
    /// <param name="command"></param>
    [EventHandler]
    public async Task DeleteUserCommandAsync(DeleteUserCommand command)
    {
        
    }
    
    /// <summary>
    /// 删除多个用户
    /// </summary>
    /// <param name="command"></param>
    [EventHandler]
    public async Task DeleteUsersCommandAsync(DeleteUsersCommand command)
    {
        
    }
}