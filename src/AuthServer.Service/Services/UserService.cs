using System.ComponentModel;

namespace AuthServer.Service.Services;

public class UserService : ApplicationService<UserService>
{
    [Description("创建用户")]
    public async Task CreateAsync(CreateUserDto createUserDto)
    {
        await EventBus.PublishAsync(new CreateUserCommand(createUserDto));
    }

    public async Task DeleteAsync(Guid id)
    {
        await EventBus.PublishAsync(new DeleteUserCommand(id));
    }

    public async Task DeleteAsync(IEnumerable<Guid> ids)
    {
        await EventBus.PublishAsync(new DeleteUsersCommand(ids));
    }
}