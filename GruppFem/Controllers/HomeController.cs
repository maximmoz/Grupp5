using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GruppFem.ServiceReference1;

namespace GruppFem.Controllers
{
    public class HomeController : Controller
    {

        ServiceReference1.Service1Client client = new Service1Client();
        ServiceReference2.LogginClient loginClient = new ServiceReference2.LogginClient();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            if (User.Identity.IsAuthenticated && this.Session["sessionUserType"].ToString() == "Sysadmin")
            {
                return View(client.GetUserInfo().ToList());

            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(string username, string password, string firstname, string lastname, string email)
        {
            client.CreateUser(username, password, firstname, lastname, email);
            return View("Index");
        }

        [HttpPost]
        public ActionResult Users(int userID, string username, string password, string firstname, string lastname, string email)
        {

            
            
                client.UpdateUser(userID, username, password, firstname, lastname, email);
                return View("Index");
            
            
            
        }

        public ActionResult Establishments()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(client.GetEstablishmentInfo((int)(System.Web.HttpContext.Current.Session["sessionID"])).ToList());
            }
            else
            {
                return View(client.GetEstablishments().ToList());
            }
            
        }
        [HttpPost]
        public ActionResult Establishments(int establishmentID, int rating, int userID)
        {

            userID = (int)(System.Web.HttpContext.Current.Session["sessionID"]);


            client.UpdateEstablishment(establishmentID, rating, userID);
            return RedirectToAction("Establishments");
        }

        public ActionResult DeleteUser(int userID)
        {
            client.DeleteUser(userID);

            return RedirectToAction("Users");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserInfo loginInfo)
        {


            if(loginInfo.Username == null || loginInfo.Password == null)
            {
                ModelState.AddModelError("", "You have to enter both username and password. ;-)");
                return View();
            }
            else
            {
                if (client.LoginUser(loginInfo.Username, loginInfo.Password) == true)
                {

                    System.Web.HttpContext.Current.Session.Add("sessionUsername", loginInfo.Username);
                    System.Web.HttpContext.Current.Session.Add("sessionID", client.GetUserID(loginInfo.Username, loginInfo.Password));
                    System.Web.HttpContext.Current.Session.Add("sessionUserType", "User");

                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(loginInfo.Username, false);
                }
                else
                {
                    ModelState.AddModelError("", "The username/password that you have entered is not correct. Try again.");
                    return View();
                }

            }
            return View();
        }

        public ActionResult LoginAdmin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(UserInfo loginInfo)
        {


            if (loginInfo.Username == null || loginInfo.Password == null)
            {
                ModelState.AddModelError("", "You have to enter both username and password. ;-)");
                return View();
            }
            else
            {
                if (loginClient.GetLogginData(loginInfo.Username, loginInfo.Password, "KodSysA").ToString() != "")
                {
                    System.Web.HttpContext.Current.Session.Add("sessionUsername", loginInfo.Username);
                    System.Web.HttpContext.Current.Session.Add("sessionUserType", "Sysadmin");
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(loginInfo.Username, false);
                }
                else
                {
                    ModelState.AddModelError("", "The username/password that you have entered is not correct. Try again.");
                    return View();
                }

            }
            return View();
        }
    }
}