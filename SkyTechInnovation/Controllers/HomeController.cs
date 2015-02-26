using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkyTechInnovation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            displayUserNameIfAvailable();
            return View();
        }

        public ActionResult About(String test)
        {
            String test1 = test;
            ViewBag.Message = "Your application description page.";
            ViewBag.SQLTest = sqlConnectionTest();
            //return RedirectToAction("Index", "Home");
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private Boolean sqlConnectionTest()
        {
            Boolean connected = false;
            String connectioinString = "server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;";
            SqlConnection testConnection = new SqlConnection(connectioinString);
            testConnection.Open();
            connected = true;
            testConnection.Close();
            return connected;
        }

        private void displayUserNameIfAvailable()
        {
            if (HttpContext.Session["currentUserId"] != null)
            {
                ViewBag.currentUser = HttpContext.Session["currentUserId"];
            }
        }
    }
}