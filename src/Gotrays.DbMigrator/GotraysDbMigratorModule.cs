using Gotrays.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Gotrays.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(GotraysEntityFrameworkCoreModule)
    )]
public class GotraysDbMigratorModule : AbpModule {

}
