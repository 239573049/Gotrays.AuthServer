namespace AuthServer.Service.Application.Users.Commands;

/// <summary>
/// 删除指定用户命令
/// </summary>
/// <param name="Id"></param>
public record DeleteUserCommand(Guid Id) : Command;

/// <summary>
/// 删除多个用户命令
/// </summary>
/// <param name="Ids"></param>
public record DeleteUsersCommand(IEnumerable<Guid> Ids) : Command;
