using System.Threading.Tasks;

namespace Gotrays.Shared;

public partial class GAppBar
{
    private void OnLogin()
    {
        Navigation.NavigateTo("Account/Login",true);
    }

    private async Task OnLogout()
	{
        Navigation.NavigateTo("/Account/Logout",true);
	}
}
