namespace AuthServer.Contracts.Users;

public class CreateUserDto
{
    /// <summary>获取或设置此用户的用户名。</summary>
    public string? UserName { get; set; }

    /// <summary>获取或设置此用户的规范化用户名。</summary>
    public string? NormalizedUserName { get; set; }

    /// <summary>获取或设置此用户的电子邮件地址。</summary>
    public string? Email { get; set; }

    /// <summary>
    /// 获取或设置此用户密码的加盐和散列表示形式。
    /// </summary>
    public string? PasswordHash { get; set; }

    /// <summary>
    /// 一个随机值，每当用户凭据更改时(密码更改，登录删除)，必须更改
    /// </summary>
    public string? SecurityStamp { get; set; }

    /// <summary>获取或设置用户的电话号码。</summary>
    public string? PhoneNumber { get; set; }
}