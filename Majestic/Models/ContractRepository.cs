using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Serialization;
using Majestic.Security;
using System.Web.Security;
using System.Web.Mvc;

namespace Majestic.Models
{
    public class ContractRepository : IContractRepository
    {
        public static void Update() { }

        public void Update(int id)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                db.Contracts.Where(p => p.ID == id).ForEach(x => x.Status = true);
                db.SaveChanges();
            }
        }

        public void SaveContract(string title)
        {
            int userId = Ext.GetUserId();

            using (RepositoryEntities db = new RepositoryEntities())
            {
                Contracts newContract = new Contracts()
                {
                    Title = title,
                    UserId = userId,
                    Status = false
                };
                db.AddToContracts(newContract);
                db.SaveChanges();
            }
        }

        public static void Delete() { }

        public void Delete(int id)
        {
            using (RepositoryEntities db = new RepositoryEntities())
            {
                db.DeleteObject(db.Contracts.Where(p => p.ID == id).First());
                db.SaveChanges();
            }
        }


        public static IEnumerable<ContractItem> GetData()
        {
            bool showAll = !System.Web.Security.Roles.GetRolesForUser().Contains("Operator");
            List<ContractItem> ret = new List<ContractItem>();
            int userId = showAll ? -1 : Ext.GetUserId();

            using (RepositoryEntities db = new RepositoryEntities())
            {
                db.Contracts.Where(w => showAll || w.UserId == userId).ForEach(x =>
                {
                    ContractItem it = new ContractItem()
                    {
                        ID = x.ID,
                        Title = x.Title,
                        Status = (null == x.Status || !(bool)x.Status) ? "Составлен" : "Утвержден",
                        User = db.Users.Where(u => u.ID == x.UserId).Select(u => u.UserLogin).First()
                    };
                    ret.Add(it);
                });
            }
            return ret;
        }
    }
}
