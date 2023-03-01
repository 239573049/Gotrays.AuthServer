using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorComponent;
using Gotrays.AuthServer.Pages.Authorization.Module;
using Masa.Blazor.Presets;

namespace Gotrays.AuthServer.Pages.Authorization;

public partial class Register
{
    private bool _show;

    private double Width { get; set; } = 500;
    private StringNumber? Elevation { get; set; } = 0;
    private string SignInRoute { get; set; } = $"Account/Login";

    private RegisterModule RegisterModule { get; set; } = new();

    /// <summary>
    /// 我同意隐私政策和条款
    /// </summary>
    private bool PrivacyPolicy { get; set; }

    private async Task SlinUpClick()
    {
        var form = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new("Input.Email", RegisterModule.Email),
                new("Input.Password", RegisterModule.Password),
                new("Input.ConfirmPassword", RegisterModule.Password)
            }
        );

        var response = await HttpClient.PostAsync(Navigation.BaseUri + "api/Register", form);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            await PopupService.EnqueueSnackbarAsync("错误", await response.Content.ReadAsStringAsync(),
                AlertTypes.Error);
        }
        else
        {
            await PopupService.EnqueueSnackbarAsync("成功", await response.Content.ReadAsStringAsync(),
                AlertTypes.Success);

            await Task.Delay(2000);

            Navigation.NavigateTo(SignInRoute);
        }
    }
}