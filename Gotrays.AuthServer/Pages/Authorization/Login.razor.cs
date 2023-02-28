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

    public LoginModule LoginModule { get; set; } = new ();
    
    private MForm _form;
    
    private async Task OnLogin()
    {
        _form.Validate();
        
        // var http = HttpClientFactory.CreateClient(string.Empty);
        //
        // // 添加一个表单数据
        // var form = new MultipartFormDataContent();
        // form.Add(new StringContent("Input.Email"), );
        
    }
}