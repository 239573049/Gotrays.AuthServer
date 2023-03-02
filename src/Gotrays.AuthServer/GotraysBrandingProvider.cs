using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Gotrays;

[Dependency(ReplaceServices = true)]
public class GotraysBrandingProvider : DefaultBrandingProvider {
    public override string AppName => "Gotrays";
}
