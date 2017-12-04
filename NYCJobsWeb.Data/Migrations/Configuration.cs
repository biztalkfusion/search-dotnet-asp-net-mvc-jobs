using NYCJobsWeb.Data.Seeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Migrations
{
    public sealed class Configuration : System.Data.Entity.Migrations.DbMigrationsConfiguration<Context.SearchContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Migrations";
        }

        protected override void Seed(Context.SearchContext context)
        {
            var instances = from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.GetInterfaces().Contains(typeof(ISeeder))
                                     && t.GetConstructor(Type.EmptyTypes) != null
                            select Activator.CreateInstance(t) as ISeeder;

            foreach (var instance in instances)
            {
                instance.Seed(context);
            }
        }
    }
}
