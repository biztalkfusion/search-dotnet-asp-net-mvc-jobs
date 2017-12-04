using NYCJobsWeb.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Seeder
{
    public interface ISeeder
    {
        void Seed(SearchContext context);
    }
}
