using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Majestic;

namespace Majestic.Security
{
    public class MyRoleProvider : RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                Users user = db.Users.FirstOrDefault(
                    u => u.UserLogin.Equals(
                        username, StringComparison.CurrentCultureIgnoreCase));

                var myRoles = from ur in db.UserRoles.Where(p => p.UserId == user.ID)
                            from r in db.Roles
                            where ur.RoleId == r.Id
                            select r.Name;

                if (myRoles != null)
                {
                    return myRoles.ToArray();
                }
                else
                {
                    return new string[] { }; ;
                }
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                Users user = db.Users.FirstOrDefault(u => u.UserLogin.Equals(username, StringComparison.CurrentCultureIgnoreCase));                    

                var roles = from ur in db.UserRoles.Where(p => p.UserId == user.ID)
                            from r in db.Roles
                            where ur.RoleId == r.Id
                            select r.Name;

                if (user != null)
                {
                    return roles.Any(r =>
                        r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                }
                else
                {
                    return false;
                }
            }
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return "Test ASP.NET MVC Application";
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
