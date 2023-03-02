using Volo.Abp.Settings;

namespace Gotrays.Settings;

public class GotraysSettingDefinitionProvider : SettingDefinitionProvider {
    public override void Define(ISettingDefinitionContext context) {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(GotraysSettings.MySetting1));
    }
}
