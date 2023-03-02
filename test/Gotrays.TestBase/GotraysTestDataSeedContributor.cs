using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Gotrays;

public class GotraysTestDataSeedContributor : IDataSeedContributor, ITransientDependency {
    public Task SeedAsync(DataSeedContext context) {
        /* Seed additional test data... */

        return Task.CompletedTask;
    }
}
