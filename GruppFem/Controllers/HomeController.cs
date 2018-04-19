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

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {

            return View(client.GetUserInfo().ToList());
        }

        public ActionResult CreateEstablishment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEstablishment(string name, string description)
        {
            client.CreateEstablishment(name, description);

            return View("Index");
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

            return View(client.GetEstablishmentInfo().ToList());
        }
        [HttpPost]
        public ActionResult Establishments(int establishmentID, string name, string description)
        {

            client.UpdateEstablishment(establishmentID, name, description);
            return RedirectToAction("Establishments");
        }

        public ActionResult DeleteUser(int userID)
        {
            client.DeleteUser(userID);

            return RedirectToAction("Users");
        }

        public ActionResult DeleteEstablishment(int establishmentID)
        {
            client.DeleteEstablishment(establishmentID);

            return RedirectToAction("Establishments");
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