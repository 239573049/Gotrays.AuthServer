using Gotrays.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Gotrays;

[DependsOn(
    typeof(GotraysEntityFrameworkCoreTestModule)
    )]
public class GotraysDomainTestModule : AbpModule {

}
