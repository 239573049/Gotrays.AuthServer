using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dantooine.Server.Data;
using Gotrays.AuthServer.Pages.Authorization.Module;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dantooine.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public LoginController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(LoginModule input, string? returnUrl = null)
    {
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result =
                await _signInManager.PasswordSignInAsync(input.Email, input.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                
                return new OkObjectResult("登录成功");
            }

            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = true });
            }

            if (result.IsLockedOut)
            {
                return new ObjectResult("您的账号被锁定")
                {
                    StatusCode = 400
                };
            }
        }

        return new ObjectResult("Invalid login attempt.")
        {
            StatusCode = 400
        };
    }
}