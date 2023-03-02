using System.Threading.Tasks;

namespace Gotrays.Data;

public interface IGotraysDbSchemaMigrator {
    Task MigrateAsync();
}
