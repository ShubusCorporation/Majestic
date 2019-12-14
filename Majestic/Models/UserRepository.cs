using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using Majestic;
using Majestic.Models;

namespace UserContracts.Models
{
    public class UserRepository : IUserRepository
    {
        private string GetHash(string inputString)
        {
            HashAlgorithm alg = SHA1.Create();
            byte[] bts = alg.ComputeHash(Encoding.UTF8.GetBytes(inputString));
            return "0x" + (BitConverter.ToString(bts).Replace("-", string.Empty)).ToUpper();
        }


        public bool CreateUser(string login, string psw, string role)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                int roleId = db.Roles.Where(p => p.Name == role).Select(p => p.Id).First();

                Users newUser = new Users()
                {
                    UserLogin = login,
                    UserPsw = GetHash(psw)
                };
                var us = db.Users.Where(p => p.UserLogin == login);

                if (us.Count() > 0)
                {
                    Users exUser = us.First();
                    bool alreadyActive = exUser.UserPsw.Length > 0;

                    if (alreadyActive)
                    {
                        return false;
                    }
                    // Восстановление удаленного
                    exUser.UserPsw = newUser.UserPsw;
                    newUser = exUser;
                }
                else
                {
                    db.AddToUsers(newUser);
                }
                db.SaveChanges();

                int newId = newUser.ID;
                UserRoles newUserRole = new UserRoles()
                {
                    RoleId = roleId,
                    UserId = newId
                };
                db.AddToUserRoles(newUserRole);
                db.SaveChanges();
            }
            return true;
        }

        public MembershipUser GetUser(string username)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                var result = from u in db.Users
                             where (string.Compare(u.UserLogin, username, true) == 0)
                             select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();
                    string _username = dbuser.UserLogin;
                    int _providerUserKey = dbuser.ID;
                    string _email = "";// dbuser.Email;
                    string _passwordQuestion = "";
                    string _comment = "";// dbuser.Comments;
                    bool _isApproved = true; // dbuser.IsActivated;
                    bool _isLockedOut = true;// dbuser.IsLockedOut;
                    DateTime _creationDate = DateTime.MinValue;// dbuser.CreatedDate;
                    DateTime _lastLoginDate = DateTime.MinValue; //dbuser.LastLoginDate;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = DateTime.MinValue; //dbuser.LastLockedOutDate;

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                            _username,
                                                            _providerUserKey,
                                                            _email,
                                                            _passwordQuestion,
                                                            _comment,
                                                            _isApproved,
                                                            _isLockedOut,
                                                            _creationDate,
                                                            _lastLoginDate,
                                                            _lastActivityDate,
                                                            _lastPasswordChangedDate,
                                                            _lastLockedOutDate);
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool Validate(string username, string password)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                var res = db.Users.Where(p => string.Compare(p.UserLogin, username, true) == 0);

                if (res == null || res.Count() == 0)
                {
                    return false;
                }
                string loginString = res.First().UserPsw;
                string checkHash = GetHash(password);

                if (loginString == null)
                {
                    return false;
                }
                return string.Compare(loginString, checkHash, true) == 0;
            }
        }


        public int GetAdminsCount()
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                return (from ur in db.UserRoles
                        from r in db.Roles
                        where ur.RoleId == r.Id && r.Name == "Administrator"
                        select ur.UserId).Count();
            }
        }


        public void UpdateUser(int id, string psw, string role)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                int roleId = db.Roles.Where(p => p.Name == role).Select(p => p.Id).First();
                string userPsw = psw == null ? psw : GetHash(psw);

                db.UserRoles.Where(p => p.UserId == id).ForEach(x => x.RoleId = roleId);

                if (psw != null)
                {
                    db.Users.Where(p => p.ID == id).ForEach(x => x.UserPsw = userPsw);
                }
                db.SaveChanges();
            }
        }


        public bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                var user = db.Users.Where(p => p.UserLogin == login);

                if (user.Count() == 0)
                {
                    return false;
                }
                int id = user.Select(p => p.ID).First();
                db.UserRoles.Where(w => w.UserId == id).ForEach(x => db.DeleteObject(x));

                if (db.Contracts.Where(p => p.UserId == id).Count() > 0)
                {
                    if (deleteAllRelatedData)
                    {
                        db.Contracts.Where(p => p.UserId == id).ForEach(x => db.DeleteObject(x));
                    }
                    else
                    {
                        db.Users.Where(p => p.UserLogin == login).First().UserPsw = "";
                    }
                }
                else
                {
                    db.DeleteObject(db.Users.Where(p => p.UserLogin == login).First());
                }
                db.SaveChanges();
            }
            return true;
        }

        public static void DeleteStub() { }

        public static void UpdateStub() { }

        public static IEnumerable<AccountItem> GetData()
        {
            List<AccountItem> ret = new List<AccountItem>();

            using (RepositoryEntities db = new RepositoryEntities())
            {/*
                var ret = (from us in db.Users
                           from rl in db.UserRoles
                           from r in db.Roles
                           where us.ID == rl.UserId && r.Id == rl.RoleId && us.UserPsw.Length > 0
                           select (new AccountItem()
                           {
                               ID = us.ID,
                               Login = us.UserLogin,
                               Password = us.UserPsw,
                               Role = r.Name
                           })).ToList();

                ret.Sort((a,b) => { return (b.ID == a.ID) ? 0 : b.ID > a.ID ? -1 : 1; });
                return ret;*/

                db.Users.Where(p => p.UserPsw.Length > 0).ForEach(x =>
                {
                    AccountItem it = new AccountItem()
                    {
                        ID = x.ID,
                        Login = x.UserLogin,
                        Password = x.UserPsw,
                        Role = string.Join(" ", System.Web.Security.Roles.GetRolesForUser(x.UserLogin)).Trim()
                    };
                    ret.Add(it);
                });
            }
            return ret;
        }
    }
}