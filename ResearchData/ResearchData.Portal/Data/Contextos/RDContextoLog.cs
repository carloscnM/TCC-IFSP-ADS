using Microsoft.EntityFrameworkCore;
using ResearchData.Portal.Models.Negocio.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResearchData.Portal.Data.Contextos
{
    public class RDContextoLog : DbContext
    {
        public RDContextoLog(DbContextOptions<RDContextoLog> options) : base(options)
        {
        }

        public DbSet<AppLog> Logging { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            UpdateUpdatedProperty<AppLog>();

            return base.SaveChanges();
        }

        private void UpdateUpdatedProperty<T>() where T : class
        {
            var modifiedSourceInfo =
                ChangeTracker.Entries<T>()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        }
    }
}
