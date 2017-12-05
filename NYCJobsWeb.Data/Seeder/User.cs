using NYCJobsWeb.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Seeder
{
    public class User : ISeeder
    {
        public void Seed(SearchContext context)
        {
            SeedUser(context);
        }

        private void SeedUser(SearchContext context)
        {
            context.Users.AddOrUpdate(x => x.Id,
                new Entities.User
                {
                    Id = 1,
                    UserName = "Admin",
                    Password = "ewd4dfwhu2BBJmSIE6RyGw==",
                    FolderName=string.Empty
                }
                //new Entities.User
                //{
                //    Id = 2,
                //    UserName = "User1",
                //    Password = "4dVKhQg3lIziM4cZA6YxMA=="
                //},
                //new Entities.User
                //{
                //    Id = 3,
                //    UserName = "User2",
                //    Password = "FqBYDNd58Z0rnoLYEIrRNQ=="
                //},
                //new Entities.User
                //{
                //    Id = 4,
                //    UserName = "User3",
                //    Password = "0YM4kFXkmDwp-FCZv5ddNg=="
                //}
                );
        }
    }
}
