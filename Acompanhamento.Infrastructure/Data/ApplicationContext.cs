using Acompanhamento.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Acompanhamento.Infrastructure.Data
{
    internal class ApplicationContext : DbContext
    {
        public ApplicationContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("DB_HOST")}; Database={Environment.GetEnvironmentVariable("DB_NAME")}; Username={Environment.GetEnvironmentVariable("DB_USER")}; Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};");

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<AcompanhamentoAggregate> Acompanhamento { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AcompanhamentoAggregate>(Entity => {
                Entity.HasKey(e => e.Id);
                Entity.Property(e => e.CodigoAcompanhamento)
                    .HasDefaultValueSql("nextval('\"Seq_CodAcompanhamento\"')");
                Entity.HasIndex(e => e.Status)
                    .HasDatabaseName("IX_Status");
            });           
        }
    }
}
