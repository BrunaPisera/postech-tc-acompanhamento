using Acompanhamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Acompanhamento.Infrastructure
{
    public static class MigrationExtensions
    {
        public static void EnsureDatabaseMigrated(this IServiceProvider provider)
        {

            var context = provider.GetRequiredService<ApplicationContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

        }

    }
}