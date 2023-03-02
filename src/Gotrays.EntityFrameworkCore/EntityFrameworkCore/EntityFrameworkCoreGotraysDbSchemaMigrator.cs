using Gotrays.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Gotrays.EntityFrameworkCore;

public class EntityFrameworkCoreGotraysDbSchemaMigrator
    : IGotraysDbSchemaMigrator, ITransientDependency {
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreGotraysDbSchemaMigrator(
        IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync() {
        /* We intentionally resolving the GotraysDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<GotraysDbContext>()
            .Database
            .MigrateAsync();
    }
}
