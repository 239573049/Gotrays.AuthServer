using System.ComponentModel.DataAnnotations;

namespace Gotrays.AuthServer.Pages.Authorization.Module;

public class LoginModule
{
    /// <summary>
    /// 邮箱
    /// </summary>
    [Required]
    [EmailAddress(ErrorMessage = "邮箱格式不正确")]
    public string Email { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$",
        ErrorMessage = "密码必须包含大小写字母、数字和特殊字符")]
    public string Password { get; set; }
}