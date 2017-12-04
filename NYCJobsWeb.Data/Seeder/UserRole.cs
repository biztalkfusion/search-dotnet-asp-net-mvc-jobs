using NYCJobsWeb.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Seeder
{
    public class UserRole : ISeeder
    {
        public void Seed(SearchContext context)
        {
            SeedUserRole(context);
        }

        private static void SeedUserRole(ISearchContext context)
        {
            context.UserRoles.AddOrUpdate(x => x.Id, new Entities.UserRole
            {
                Id = 1,
                UserId = 1,
                RoleId = 1
            },
            new Entities.UserRole
            {
                Id = 2,
                UserId = 2,
                RoleId = 2
            },
            new Entities.UserRole
            {
                Id = 3,
                UserId = 3,
                RoleId = 2
            },
            new Entities.UserRole
            {
                Id = 4,
                UserId = 4,
                RoleId = 2
            });
        }
    }
}
