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
                },
                new Entities.User
                {
                    Id = 2,
                    UserName = "CLLQ",
                    Password = "4dVKhQg3lIziM4cZA6YxMA==",
                    FolderName = "003CLLQ"
                },
                new Entities.User
                {
                    Id = 3,
                    UserName = "DLSS",
                    Password = "FqBYDNd58Z0rnoLYEIrRNQ==",
                    FolderName = "003DLSS"
                },
                new Entities.User
                {
                    Id = 4,
                    UserName = "EXLA",
                    Password = "0YM4kFXkmDwp-FCZv5ddNg==",
                    FolderName = "003EXLA"
                },
                new Entities.User
                {
                    Id = 5,
                    UserName = "SHAF",
                    Password = "2m5FA5PCXdcySRC825KxOg==",
                    FolderName = "008SHAF"
                }
                );
        }
    }
}
