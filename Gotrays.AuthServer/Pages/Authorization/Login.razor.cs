using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorComponent;
using Gotrays.AuthServer.Pages.Authorization.Module;
using Masa.Blazor;
using Microsoft.AspNetCore.Components;

namespace Gotrays.AuthServer.Pages.Authorization;

public partial class Login
{
    private bool _show;

    [Parameter] public bool HideLogo { get; set; } = true;

    [Parameter] public double Width { get; set; } = 500;

    [Parameter] public StringNumber? Elevation { get; set; }

    [Parameter] public string CreateAccountRoute { get; set; } = $"Account/Register";

    [Parameter] [SupplyParameterFromQuery] public string ReturnUrl { get; set; }

    private LoginModule LoginModule { get; } = new();

    private MForm _form;

    private async Task OnLogin()
    {
        _form.Validate();
        
        var result =
            await HttpClient.PostAsJsonAsync(NavigationManager.BaseUri + "api/Login" + ReturnUrl,
                LoginModule);

        result.Headers.ForEach(x =>
        {
            if (x.Key.ToLower().Contains("Cookie"))
            {
                
            }
        });
        
        if (result.IsSuccessStatusCode)
        {
            await PopupService.EnqueueSnackbarAsync("成功", "登录成功", AlertTypes.Success);
            
            var token = await JSRuntime.InvokeAsync<string>("getAntiForgeryToken",null);
            
            var a = await HttpClient.GetStringAsync(NavigationManager.BaseUri + "connect/userinfo");
        }
        else
        {
            await PopupService.EnqueueSnackbarAsync("登录失败", await result.Content.ReadAsStringAsync(), AlertTypes.Error);
        }
    }
}