using SkyTechInnovation.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SkyTechInnovation.Controllers
{
    public class SlideShowController : Controller
    {
        //
        // GET: /SlideShow/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Pics/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index");
        }

        public ActionResult SlideShowControl(String id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public ActionResult SlideShowControl(String Id, HttpPostedFileBase file)
        {
            String id = Id;
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Pics/uploads"), fileName);
                file.SaveAs(path);
                path = Path.Combine("/Pics/uploads", fileName);
                updateSlideShowParm(id, Convert.ToString(path));
            }
            return RedirectToAction("SlideShowBrowse", "SlideShow");
        }

        public ActionResult SlideShowBrowse()
        {
            List<SlideShow> slideShows = getSlideShows();
            return View(slideShows);
        }

        private List<SlideShow> getSlideShows()
        {
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("select * from anbbvv_sti.jasper.SlideShow", connection);
            List<SlideShow> slideShows = new List<SlideShow>();
            connection.Open();
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    SlideShow slideShow = new SlideShow();
                    slideShow.Id = dataReader.GetString(0).Trim();
                    slideShow.Path = dataReader.GetString(1).Trim();
                    slideShows.Add(slideShow);
                }
            }
            connection.Close();
            return slideShows;
        }

        private List<SlideShow> updateSlideShowParm(String id, String path)
        {
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("update anbbvv_sti.jasper.SlideShow set path = '"+path+"' where id = '"+id+"'", connection);
            List<SlideShow> slideShows = new List<SlideShow>();
            connection.Open();
            command.ExecuteReader();
            connection.Close();
            return slideShows;
        }
	}
}

