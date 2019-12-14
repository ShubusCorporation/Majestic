using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Majestic.Models
{
    public interface IUserRepository
    {
        bool CreateUser(string login, string psw, string role);
        MembershipUser GetUser(string username);
        bool Validate(string username, string password);
        void UpdateUser(int id, string psw, string role);
        bool DeleteUser(string login, bool deleteAllRelatedData);
    }
}
