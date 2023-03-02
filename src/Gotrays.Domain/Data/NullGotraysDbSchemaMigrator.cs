using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Gotrays.Data;

/* This is used if database provider does't define
 * IGotraysDbSchemaMigrator implementation.
 */
public class NullGotraysDbSchemaMigrator : IGotraysDbSchemaMigrator, ITransientDependency {
    public Task MigrateAsync() {
        return Task.CompletedTask;
    }
}
