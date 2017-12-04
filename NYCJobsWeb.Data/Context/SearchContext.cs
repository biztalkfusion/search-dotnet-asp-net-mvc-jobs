using NYCJobsWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Context
{
    public class SearchContext : DbContext, ISearchContext
    {
        public SearchContext() : base("SearchContext")
        {
            Database.SetInitializer(new CheckAndMigrateDatabaseToLatestVersion<SearchContext, Migrations.Configuration>());
        }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Modified;
            SaveChanges();
        }

        public void Delete<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Deleted;
            SaveChanges();
        }
    }

    public class CheckAndMigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>
  : IDatabaseInitializer<TContext>
  where TContext : DbContext
  where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        public virtual void InitializeDatabase(TContext context)
        {
            var migratorBase = ((MigratorBase)new DbMigrator(Activator.CreateInstance<TMigrationsConfiguration>()));
            if (migratorBase.GetPendingMigrations().Any())
                migratorBase.Update();
        }
    }
}
