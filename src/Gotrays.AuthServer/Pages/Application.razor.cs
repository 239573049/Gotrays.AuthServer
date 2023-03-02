using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.OpenIddict.Applications;

namespace Gotrays.Pages;

public partial class Application
{
    public List<OpenIddictApplication> Applications { get; protected set; }

    protected override async Task OnInitializedAsync()
    {
        Applications = await OpenIddictApplicationRepository.GetListAsync();

        await base.OnInitializedAsync();
    }

    private async Task OnGoto(OpenIddictApplication application)
    {
        await JSRuntime.InvokeVoidAsync("window.open",application.ClientUri);
    }
}
