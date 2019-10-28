using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ShoppingCartSample.Models;
using ShoppingCartData;

namespace ShoppingCartSample.Controllers
{
    public class HomeController : Controller
    {
        ShoppingCartSampleEntities context = new ShoppingCartSampleEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Product()
        {
            ViewBag.Message = "Your Import page.";

            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "CATALOG";
            xRoot.IsNullable = true;
            XmlSerializer deserializer = new XmlSerializer(typeof(List<CD>), xRoot);
            TextReader textReader = new StreamReader(@"C:\Harry\eBooks\Eranga\ShoppingCartSample\Product.xml");
            List<CD> movies;
            movies = (List<CD>)deserializer.Deserialize(textReader);
            ViewData["movies"] = movies;
            return View(movies);
        }

        [HttpPost]
        public ActionResult Import()
        {
            foreach (CD movie in ViewData["movies"] as IList<CD>)
            {
                Product pd = new Product();
                pd.Title = movie.TITLE;
                pd.Artist = movie.ARTIST;
                pd.Company = movie.COMPANY;
                pd.Country = movie.COUNTRY;
                pd.Price = movie.PRICE;
                pd.Year = movie.YEAR;
                context.Products.Add(pd);
            }
            context.SaveChanges();

            var productList = context.Products.ToList();

            return View(productList);
        }
    }
}