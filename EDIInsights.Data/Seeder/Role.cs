using BizTalkFusion.Solutions.Integration.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkFusion.Solutions.Integration.Data.Seeder
{
    public class Role : ISeeder
    {
        public void Seed(SearchContext context)
        {
            SeedRole(context);
        }

        private static void SeedRole(ISearchContext context)
        {
            context.Roles.AddOrUpdate(x => x.Id,
                new Entities.Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Entities.Role
                {
                    Id = 2,
                    Name = "User"
                });
        }
    }
}
