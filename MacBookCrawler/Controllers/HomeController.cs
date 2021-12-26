using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MacBookCrawler.Models;

namespace MacBookCrawler.Controllers
{
    public class HomeController : Controller
    {
        Model m = new Model(); //For data manipulation on DB tables, Model representation variable defined global for use all method.

        public List<Product> InıtializeData(int? check)
        {
            List<Product> UrunlerAll = new List<Product>();

            List<Product> UrunlerAmz = new List<Product>();
            List<Product> UrunlerN11 = new List<Product>();
            List<Product> UrunlerTrendyol = new List<Product>();
            List<Product> urunlerTeknosa = new List<Product>();

            if (m.Products.ToList().Count != 0 && check == null)//if db tables are not null or empty;
            {
                UrunlerAll = m.Products.ToList();
            }    
            else//if data tables are null or empty, fetch all website datas again;
            {
                FetchAmazon amazonsMac = new FetchAmazon();
                FetchN11 n11sMac = new FetchN11();
                FetchTrendyol trendyolsMac = new FetchTrendyol();
                FetchTeknosa teknosasMac = new FetchTeknosa();


                UrunlerAmz = new List<Product>(amazonsMac.FetchProducts());
                UrunlerN11 = new List<Product>(n11sMac.FetchProducts());
                UrunlerTrendyol = new List<Product>(trendyolsMac.FetchProducts());
                urunlerTeknosa = new List<Product>(teknosasMac.FetchProducts());

                UrunlerAll.AddRange(UrunlerAmz);
                UrunlerAll.AddRange(UrunlerN11);
                UrunlerAll.AddRange(UrunlerTrendyol);
                UrunlerAll.AddRange(urunlerTeknosa);
            }

            
            ViewBag.amazonCounter = UrunlerAll.Where(x => x.Firm == "Amazon").ToList().Count.ToString();
            ViewBag.n11Counter = UrunlerAll.Where(x => x.Firm == "N11").ToList().Count.ToString();
            ViewBag.trendyolCounter = UrunlerAll.Where(x => x.Firm == "Trendyol").ToList().Count.ToString();
            ViewBag.teknosaCounter = UrunlerAll.Where(x => x.Firm == "Teknosa").ToList().Count.ToString();

            foreach(Product itemPr in UrunlerAll)
            {
                var input = itemPr.Price;
                itemPr.Price = new string(input.Where(c => char.IsDigit(c)).ToArray());
            }

            return UrunlerAll;
        }


        public ActionResult Index()
        {
            if ((string)TempData["Admin"] == "1")//If this is a admin acces;
                ViewBag.Admin = "1";//Make a temp variable not null in order to showing control panel of app.

            try
            {
                List<Product> Urunler = new List<Product>(InıtializeData(null));
                ViewBag.TotalMacs = Urunler.Count.ToString();
                ViewBag.Alert = ViewBag.TotalMacs + " Product listed";
                return View(Urunler);
            }
            catch (Exception)
            {
                ViewBag.Alert = "Error occured because of responsing server or XPATHs. Please try again later.";
                return View();
            }

        }

        [HttpPost]
        public ActionResult FilterFetch(int? minPrice, int? maxPrice, String titleCont, String[] Mplaces)
        {
            int anyChanges = 0;//Changes detecting variable
            int minPrices = 0, maxPrices = 0;//Defining min and max prices.
            if (minPrice != 0 && minPrice != null)
                minPrices = Convert.ToInt32(minPrice);
            if (maxPrice != 0 && maxPrice != null)
                maxPrices = Convert.ToInt32(maxPrice);
            //With getting posted min and max price values.

            List<Product> Urunler = new List<Product>(InıtializeData(null));
            //Fetching goods in a list that named Urunler.

            if (titleCont != null && titleCont != "")//If any title search, applying..
            {
                Urunler = Urunler.Where(x => x.Name.ToLower().Contains(titleCont.ToLower())).ToList();
                anyChanges++;
            }

            switch (Mplaces.Length)
            {
                case 1:
                    Urunler = Urunler.Where(x => x.Firm.Contains(Mplaces[0])).ToList();
                    anyChanges++;
                    break;
                case 2:
                    Urunler = Urunler.Where(x => x.Firm.Contains(Mplaces[0])
                    || x.Firm.Contains(Mplaces[1])).ToList();
                    anyChanges++;
                    break;
                case 3:
                    Urunler = Urunler.Where(x => x.Firm.Contains(Mplaces[0])
                    || x.Firm.Contains(Mplaces[1])
                    || x.Firm.Contains(Mplaces[2])).ToList();
                    anyChanges++;
                    break;
                case 4:
                    Urunler = Urunler.Where(x => x.Firm.Contains(Mplaces[0])
                    || x.Firm.Contains(Mplaces[1])
                    || x.Firm.Contains(Mplaces[2])
                    || x.Firm.Contains(Mplaces[3])).ToList();
                    break;
                default:
                    break;
            }

            List<Product> tempUrunler = new List<Product>();

            int tempPrice = 0;
            if(minPrices != 0 && maxPrices != 0 && (minPrices < maxPrices))
            {
                foreach(Product item in Urunler)
                {
                    tempPrice = Convert.ToInt32(item.Price);

                    if (item.Price.Length == 7 && Int32.Parse(item.Price) >= 10000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 5));
                    else if (item.Price.Length == 6 && Int32.Parse(item.Price) >= 1000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 4));
                    else if (item.Price.Length == 8 && Int32.Parse(item.Price) >= 100000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 6));

                    if (tempPrice < maxPrices && tempPrice > minPrices)
                        tempUrunler.Add(item);
                }
                ViewBag.Alert += "Minimum and Maximum Price Filtering has applied. ";
            }
            else if (minPrices != 0)
            {
                foreach (Product item in Urunler)
                {
                    tempPrice = Convert.ToInt32(item.Price);

                    if (item.Price.Length == 7 && Int32.Parse(item.Price) >= 10000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 5));
                    else if (item.Price.Length == 6 && Int32.Parse(item.Price) >= 1000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 4));
                    else if (item.Price.Length == 8 && Int32.Parse(item.Price) >= 100000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 6));

                    if (tempPrice > minPrices)
                        tempUrunler.Add(item);
                }
                ViewBag.Alert += "Minimum Price Filtering has applied. ";
            }

            else if (maxPrices != 0)
            {
                foreach (Product item in Urunler)
                {
                    tempPrice = Convert.ToInt32(item.Price);

                    if (item.Price.Length == 7 && Int32.Parse(item.Price) >= 10000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 5));
                    else if (item.Price.Length == 6 && Int32.Parse(item.Price) >= 1000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 4));
                    else if (item.Price.Length == 8 && Int32.Parse(item.Price) >= 100000)
                        tempPrice = Int32.Parse(item.Price.Substring(0, 6));

                    if (tempPrice < maxPrices)
                        tempUrunler.Add(item);
                }
                ViewBag.Alert += "Maximum Price Filtering has applied. ";
            }

            if (tempUrunler.Count != 0)
            {
                Urunler = tempUrunler;
                ViewBag.Alert += Urunler.Count.ToString() + " Product listed.";
            }
            else if (tempUrunler.Count == 0 && anyChanges == 0)
                ViewBag.Alert = "Filters has been applied but there are not any changes. " + Urunler.Count.ToString() + " products listed again.";



            ViewBag.TotalMacs = Urunler.Count.ToString();
            
            ViewBag.amazonCounter = Urunler.Where(x => x.Firm == "Amazon").ToList().Count.ToString();
            ViewBag.n11Counter = Urunler.Where(x => x.Firm == "N11").ToList().Count.ToString();
            ViewBag.trendyolCounter = Urunler.Where(x => x.Firm == "Trendyol").ToList().Count.ToString();
            ViewBag.teknosaCounter = Urunler.Where(x => x.Firm == "Teknosa").ToList().Count.ToString();
            return View("Index",Urunler);
        }

        public ActionResult SaveData()
        {
            try
            {
                List<Product> Urunler = new List<Product>(InıtializeData(1));
                //Fetching goods in a list that named Urunler.

                ViewBag.TotalMacs = Urunler.Count.ToString();
                ViewBag.Alert = "Up-to-date data of the products were obtained and saved. " + ViewBag.TotalMacs + " Product listed";

                //Removing all datas on DB table;
                Model m = new Model();
                m.Products.RemoveRange(m.Products.ToList());
                m.SaveChanges();

                //Saving new datas on DB table;
                foreach (Product item in Urunler)
                {
                    item.LastUpdated = DateTime.Now;
                    var input = item.Price;
                    item.Price = new string(input.Where(c => char.IsDigit(c)).ToArray());

                    m.Products.Add(item);
                }

                m.SaveChanges();


                return View("Index", Urunler);
            }
            catch (Exception)
            {
                ViewBag.Alert = "Error occured because of responsing server or XPATHs. Please try again later.";
                return View("Index");
            }
        }

        public ActionResult FetchAgain()
        {
            try
            {
                List<Product> Urunler = new List<Product>(InıtializeData(1));
                //Fetching goods in a list that named Urunler.

                ViewBag.TotalMacs = Urunler.Count.ToString();
                ViewBag.Alert = "Up-to-date data of the products were obtained. " + ViewBag.TotalMacs + " Product listed";
                return View("Index", Urunler);
            }
            catch (Exception)
            {
                ViewBag.Alert = "Error occured because of responsing server or XPATHs. Please try again later.";
                return View("Index");
            }
        }
    }
}