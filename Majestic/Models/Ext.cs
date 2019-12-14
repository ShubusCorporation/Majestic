using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Majestic.Models
{
    public static class Ext
    {
        public static void ForEach<T>(this IQueryable<T> item, Action<T> act)
        {
            foreach (var x in item)
            {
                act(x);
            }
        }

        public static int GetUserId()
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                string login = Membership.GetUser().UserName;
                int userId = db.Users.Where(p => p.UserLogin == login).Select(p => p.ID).First();
                return userId;
            }
        }
    }
}
