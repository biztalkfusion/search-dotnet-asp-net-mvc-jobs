using NYCJobsWeb.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NYCJobsWeb.Data.Context
{
    public interface ISearchContext
    {
        int SaveChanges();
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        IDbSet<Role> Roles { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<UserRole> UserRoles { get; set; }
    }
}
