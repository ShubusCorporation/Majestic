using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Majestic.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [Authorize(Roles = "Administrator, Coordinator, Operator")]
        public ActionResult ContractsView()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult ContractsView(string contract)
        {
            if (contract != null)
            {
                if (string.IsNullOrEmpty(contract.Trim()))
                {
                    ModelState.AddModelError("contract", "You must specify contract name.");
                }
                else
                {
                    MyContractProvider pr = new MyContractProvider();
                    pr.SaveContract(contract);
                }
            }
            return View();
        }


        [Authorize(Roles = "Administrator")]
        public ActionResult AccountsView()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult AccountsView(string userName, string password, object ddList)
        {
            if (string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password))
            {
                return View();
            }
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                try
                {
                    Membership.CreateUser(userName, password, (ddList as string[])[0]);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("username", ex.Message);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("username", "You must specify a username.");
                }
                else
                {
                    ModelState.AddModelError("password", "You must specify a password..");
                }
            }
            return View();
        }
       
        public ActionResult About()
        {
            return View();
        }
    }
}
