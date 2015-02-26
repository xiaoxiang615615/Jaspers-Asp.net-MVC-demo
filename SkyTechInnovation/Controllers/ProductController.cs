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
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        public ActionResult Index(String Id)
        {
            String page;
            if(Id == null)
            {
                page = "1";
            }
            else
            {
                page = Id;
            }

            
            return View();
        }

        private List<ProductBrowse> getProductsWithPage(String page)
        {
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("select * from anbbvv_sti.jasper.PRODUCT where id >  " + ((Convert.ToInt32(page) - 1) * 12).ToString() + " and id <= " + (Convert.ToInt32(page) * 12).ToString(), connection);
            List<ProductBrowse> products = new List<ProductBrowse>();
            connection.Open();
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    ProductBrowse product = new ProductBrowse();
                    product.Id = dataReader.GetString(0).Trim();
                    product.Name = dataReader.GetString(1).Trim();
                    product.ImgPath = dataReader.GetString(2).Trim();
                    products.Add(product);
                }
            }
            connection.Close();
            return products;
        }

     

        public ActionResult ProductControlBrowse()
        {
            List<ProductBrowse> products = getProducts();
            return View(products);
        }

        public ActionResult ProductControlDetail(String Id)
        {


            return View();
        }

        public ActionResult ProductEdit(String id)
        {
            ProductDetail productDetail = getProductWithId(id);
            @ViewBag.name = productDetail.name;
            @ViewBag.prodImg = productDetail.prodImg;
            @ViewBag.itno = productDetail.itno;
            @ViewBag.cost = productDetail.cost;
            @ViewBag.note = productDetail.note;
            @ViewBag.price = productDetail.price;
            @ViewBag.saleDesc = productDetail.saleDesc;
            @ViewBag.cate = productDetail.cate;
            @ViewBag.id = productDetail.id;

            return View();
        }

        public ActionResult ProductDelete(String id)
        {
            deleteProductWithId(id);
            return RedirectToAction("ProductControlBrowse");
        }

        public ActionResult CreateProduct()
        {
            return View();
        }

        public ActionResult Browse(String id)
        {
            //To create pager
            List<Page> pages = new List<Page>();
            String currentPage = id;
            String allPages = (Convert.ToInt32(getProductsCount()) / 12+1).ToString();
            if (id == null)
            {
                currentPage = "1";
            }
            if (Convert.ToInt32(allPages) >= 5)
            {
                if (Convert.ToInt32(currentPage) + 2 <= Convert.ToInt32(allPages))
                {
                    if (Convert.ToInt32(currentPage) >= 3)
                    {
                        for (int i = -2; i <= 2; i++)
                        {
                            Page page = new Page();
                            page.page = (Convert.ToInt32(id) + i).ToString();
                            if (currentPage == page.page)
                            {
                                page.active = true;
                            }
                            else
                            {
                                page.active = false;
                            }
                            pages.Add(page);
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 5; i++)
                        {
                            Page page = new Page();
                            page.page = i.ToString();
                            if (currentPage == page.page)
                            {
                                page.active = true;
                            }
                            else
                            {
                                page.active = false;
                            }
                            pages.Add(page);
                        }
                    }
                }
                else
                {
                    for (int i = 4; i >= 0; i--)
                    {
                        Page page = new Page();
                        page.page = (Convert.ToInt32(allPages) - i).ToString();
                        if (currentPage == page.page)
                        {
                            page.active = true;
                        }
                        else
                        {
                            page.active = false;
                        }
                        pages.Add(page);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= Convert.ToInt32(allPages); i++)
                {
                    Page page = new Page();
                    page.page = i.ToString();
                    if (currentPage == page.page)
                    {
                        page.active = true;
                    }
                    else
                    {
                        page.active = false;
                    }
                    pages.Add(page);
                }
            }

            if (Convert.ToInt32(currentPage) > 1)
            {
                ViewBag.previousPage = Convert.ToInt32(currentPage) - 1;
            }
            else
            {
                ViewBag.previousPage = 1;
            }
            if (Convert.ToInt32(currentPage) < Convert.ToInt32(allPages))
            {
                ViewBag.nextPage = Convert.ToInt32(currentPage) + 1;
            }
            else
            {
                ViewBag.nextPage = allPages;
            }
            ViewData["pages"] = pages;

            List<ProductBrowse> products = getProductsBasedOnPage(currentPage, "12");
            ViewData["productsToShow"] = products;

            return View();
        }

        private List<ProductBrowse> getProductsBasedOnPage(String page, String numberOfItems)
        {
            String startPoint = ((Convert.ToInt32(page) - 1) * Convert.ToInt32(numberOfItems)).ToString();
            String subQueryPoint = (Convert.ToInt32(startPoint) + Convert.ToInt32(numberOfItems)).ToString();
            String productsCount = getProductsCount();
            String topItems = "";
            if(Convert.ToInt32(productsCount) - Convert.ToInt32(startPoint) < Convert.ToInt32(numberOfItems))
            {
                topItems = Convert.ToString(Convert.ToInt32(productsCount) - Convert.ToInt32(startPoint));
            }
            else
            {
                topItems = numberOfItems;
            }
            
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("select * from (select top " + topItems + " * from (select top " + subQueryPoint + " * from anbbvv_sti.jasper.PRODUCT order by ID) as T order by ID desc) as A order by ID", connection);
            List<ProductBrowse> products = new List<ProductBrowse>();
            int counter = 0;
            connection.Open();
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    ProductBrowse productBrowse = new ProductBrowse();
                    productBrowse.Name = dataReader.GetString(1).Trim();
                    productBrowse.Id = Convert.ToString(dataReader.GetValue(0));
                    productBrowse.ImgPath = dataReader.GetString(2).Trim();
                    products.Add(productBrowse);
                    counter++;
                }
            }
            connection.Close();
            return products;
        }

        private List<ProductBrowse> getProducts()
        {
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("select * from anbbvv_sti.jasper.PRODUCT", connection);
            List<ProductBrowse> products = new List<ProductBrowse>();
            connection.Open();
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    ProductBrowse product = new ProductBrowse();
                    product.Id = Convert.ToString(dataReader.GetValue(0));
                    product.Name = dataReader.GetString(1).Trim();
                    product.ImgPath = dataReader.GetString(2).Trim();
                    products.Add(product);
                }
            }
            connection.Close();
            return products;
        }

        private String getProductsCount()
        {
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("select count(*) from anbbvv_sti.jasper.PRODUCT", connection);
            connection.Open();
            String data = "";
            int counter = 0;
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {

                    data = Convert.ToString(dataReader.GetInt32(0)).Trim();

                }
            }
            connection.Close();
            return data;
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult BrowseDJI()
        {
            return View();
        }

        public ActionResult BrowseDIY()
        {
            return View();
        }

        public ActionResult CreateProductSQLAction()
        {
            return View();
        }

        

        [HttpPost]
        public ActionResult CreateProductSQLAction(String name, String no, String cost, String note, String price, String proddesc, String cate, HttpPostedFileBase file)
        {
            var test = Request.Files;
            String filePath = "";
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Pics/uploads"), fileName);
                file.SaveAs(path);
                path = Path.Combine("/Pics/uploads", fileName);
                filePath = path.ToString();
            }
            insertIntoPRODUCT(name, filePath, no, cost, note, price, proddesc, cate);
            return RedirectToAction("ProductControlBrowse");
        }

        [HttpPost]
        public ActionResult UpdateProductSQLAction(String name, String no, String id, String cost, String note, String price, String proddesc, String cate, HttpPostedFileBase file)
        {
            var test = Request.Files;
            String filePath = "";
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Pics/uploads"), fileName);
                    file.SaveAs(path);
                    path = Path.Combine("/Pics/uploads", fileName);
                    filePath = path.ToString();
                }
            }
            else
            {
                filePath = "";
            }
            //deleteProductWithId(id);
            //insertIntoPRODUCTWithId(id, name, filePath, no, cost, note, price, proddesc, cate);
            updatePRODUCTWithId(id, name, filePath, no, cost, note, price, proddesc, cate);
            return RedirectToAction("ProductControlBrowse");
            //return RedirectToAction("ProductControlBrowse");
        }


        private Boolean insertIntoPRODUCT(String name, String filePath, String no, String cost, String note, String price, String proddesc, String cate)
        {
            try
            {
                SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
                SqlCommand command = new SqlCommand("insert into anbbvv_sti.jasper.PRODUCT values ('" + name + "', '" + filePath + "', '" + no + "', '" + cost + "', '" + note + "', '" + price + "', '" + proddesc + "', '" + cate + "')", connection);
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

        private Boolean updatePRODUCTWithId(String id, String name, String filePath, String no, String cost, String note, String price, String proddesc, String cate)
        {
            try
            {
                SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
                SqlCommand command = new SqlCommand("update anbbvv_sti.jasper.PRODUCT set NAME = '"+name+"', PRODIMG = '"+filePath+"', ITNO = '"+no+"', COST = '"+cost+"', NOTE = '"+note+"', PRICE = '"+price+"', SALEDESC = '"+proddesc+"', CATE = '"+cate+"' where ID = '"+id+"'", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private Boolean insertIntoPRODUCTWithId(String id, String name, String filePath, String no, String cost, String note, String price, String proddesc, String cate)
        {
            try
            {
                SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
                SqlCommand command = new SqlCommand("insert into anbbvv_sti.jasper.PRODUCT values ('" + id + "', '" + name + "', '" + filePath + "', '" + no + "', '" + cost + "', '" + note + "', '" + price + "', '" + proddesc + "', '" + cate + "')", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private Boolean deleteProductWithId(String id)
        {
            try
            {
                SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
                SqlCommand command = new SqlCommand("delete from anbbvv_sti.jasper.PRODUCT where ID = '"+id+"'", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private ProductDetail getProductWithId(String id)
        {
            SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
            SqlCommand command = new SqlCommand("select * from anbbvv_sti.jasper.PRODUCT where ID = '"+id+"'", connection);
            ProductDetail product = new ProductDetail();
            connection.Open();
            using (SqlDataReader dataReader = command.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    product.id = Convert.ToString(dataReader.GetValue(0));
                    product.name = dataReader.GetString(1).Trim();
                    product.prodImg = dataReader.GetString(2).Trim();
                    product.itno = dataReader.GetString(3).Trim();
                    product.cost = dataReader.GetValue(4).ToString();
                    product.note = dataReader.GetString(5).Trim();
                    product.price = dataReader.GetValue(6).ToString();
                    product.saleDesc = dataReader.GetString(7).Trim();
                    product.cate = dataReader.GetString(8).Trim();
                }
            }
            connection.Close();
            return product;
        }

        //private String generateIdForProduct()
        //{
        //    SqlConnection connection = new SqlConnection("server=184.168.47.19;database=anbbvv_sti;user id=jasper;password=wenwen927;");
        //    SqlCommand command = new SqlCommand("select max(ID) from anbbvv_sti.jasper.PRODUCT", connection);
        //    connection.Open();
        //    String data = "";
        //    int counter = 0;
        //    using (SqlDataReader dataReader = command.ExecuteReader())
        //    {
        //        while (dataReader.Read())
        //        {

        //            data = dataReader.GetValue(0).ToString();

        //        }
        //    }
        //    connection.Close();
        //    return Convert.ToString(Convert.ToInt32(data)+1);
        //}
       
	}
}