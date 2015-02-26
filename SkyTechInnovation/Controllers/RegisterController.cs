using SkyTechInnovation.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SkyTechInnovation.Controllers
{
    public class RegisterController : Controller
    {

        private Boolean goToHome = false;
        //
        // GET: /Register/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterDetail()
        {
            if(goToHome == true)
            {
                RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult RegisterDetail(String userName, String passWord, String email)
        {
           // String username = userName;
           // String password = passWord;
           // String Email = email;
           //// insertIntoUser(username, password, Email);
           // ViewBag.message = "Register is successful";

            return RedirectToAction("Index", "Home");
            return View();
        }

        private Boolean insertIntoUser(String userName, String passWord, String Email)
        {
            Boolean connected = false;
            String connectioinString = "server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;";
            SqlConnection sqlConnection = new SqlConnection(connectioinString);
            SqlCommand insertStatement = new SqlCommand("INSERT INTO [anbbvv_sti].[jasper].[USER] (USID, PSWD, EMAL) VALUES ('" + userName + "', '" + passWord + "', '" + Email + "')", sqlConnection);
            sqlConnection.Open();
            insertStatement.ExecuteReader();
            connected = true;
            sqlConnection.Close();
            return connected;
        }

        public ActionResult RegisterSuccessful()
        {
       
            return View();
        }

        [HttpPost]
        public ActionResult RegisterSuccessful(String test)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult processRegister(String userName, String passWord, String email)
        {
            String username = userName;
            String password = passWord;
            String Email = email;
            insertIntoUser(username, password, Email);
            ViewBag.message = "Register is successful";
            this.goToHome = true;
            saveUserIdIntoSession(username);
            return RedirectToAction("Index", "Home");
        }

        private Boolean saveUserIdIntoSession(String username)
        {
            HttpContext.Session["currentUserId"] = username;
            return true;
        }

        private void displayUserNameIfAvailable()
        {
            if(HttpContext.Session["currentUserId"] != null)
            {
                ViewBag.currentUser = HttpContext.Session["currentUserId"];
            }
        }
	}
}