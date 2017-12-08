using BizTalkFusion.Solutions.Integration.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizTalkFusion.Solutions.Integration.Data.Seeder
{
    public interface ISeeder
    {
        void Seed(SearchContext context);
    }
}
