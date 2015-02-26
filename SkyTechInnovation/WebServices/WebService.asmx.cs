using SkyTechInnovation.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace SkyTechInnovation.WebServices
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public String getSlideShows(String test)
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
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(slideShows);
        }

        [WebMethod]
        public Boolean SendEnquiryEmail(String name, String email, String phone, String enquiry)
        {

            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential =
                new NetworkCredential("sales@skytechinnovation.com.au", "Password01");
            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("sales@skytechinnovation.com.au");

            smtpClient.Host = "smtpout.secureserver.net";
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;

            message.From = fromAddress;
            message.Subject = "SkyTechInno Sales Enquiry";
            //Set IsBodyHtml to true means you can send HTML email.
            message.IsBodyHtml = true;
            message.Body = "<h4>Name : " + name + "</h4><br />" + "<h4>Email : " + email + "</h4><br />" + "<h4>Phone : " + phone + "</h4><br />" + "<h4>Enquiry : " + enquiry + "</h4><br />";
            message.To.Add("skytechinnovationau@gmail.com");

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                //Error, could not send the message

            }

            return true;
        }

        [WebMethod]
        public Boolean CreateProduct(String name, String prodimg, String itno, String cost, String note, String price, String saledesc, String cate)
        {
            try
            {
                SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
                SqlCommand command = new SqlCommand("insert into anbbvv_sti.jasper.PRODUCT values ('" + name + "', '" + prodimg + "', '" + itno + "', '" + cost + "', '" + note + "', '" + price + "', '" + saledesc + "', '" + cate + "')", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

    }
}
