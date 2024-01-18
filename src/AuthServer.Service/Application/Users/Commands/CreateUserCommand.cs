namespace AuthServer.Service.Application.Users.Commands;

/// <summary>
/// 创建用户命令
/// </summary>
/// <param name="CreateUserDto"></param>
public record CreateUserCommand(CreateUserDto CreateUserDto) : Command;
