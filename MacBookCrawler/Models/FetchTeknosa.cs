using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MacBookCrawler.Models
{
    public class FetchTeknosa
    {
        public List<Product> FetchProducts()
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.teknosa.com/macbook-c-102010103");

            List<Product> Urunler = new List<Product>();

            //1-Getting Product Title.
            int i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//*//div/div/div/div/div/div[@class='product-name']/a//span"))
            {
                Urunler.Add(new Product());

                Urunler[i].Name = item.InnerText.ToString().Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "/").Replace("&quot;", "Inc").Replace("&#x27;", "'").Replace("&#34;", "Inc");
                Urunler[i].Firm = "Teknosa";//Firm name giving manually

                i++;
            }

            //2-Getting Price Value
            i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//*//div/div/div/div/div/div[@class='product-price visible-grid-view clearfix']//span"))
            {
                if (i < Urunler.Count)
                {
                    Urunler[i].Price = item.InnerText.Substring(0, item.InnerText.Length - 2);
                    i++;
                }
            }

            //3-Getting Direct Img Link
            i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//*//div/div/div[@class='product-image-item']//a"))
            {
                if (i < Urunler.Count)
                {
                    var tempDoc = web.Load("https://teknosa.com" + item.Attributes["href"].Value.ToString());

                    if (tempDoc.DocumentNode.SelectNodes("//*//a//img[@class='product-detail-image']") != null)
                    {
                        foreach (var itemx in tempDoc.DocumentNode.SelectNodes("//*//a//img[@class='product-detail-image']"))
                        {
                            Urunler[i].ImageURL = itemx.Attributes["src"].Value.ToString();
                            break;
                        }
                    }
                    i++;
                }
            }

            //4-Getting Direct Link
            i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//*//div/div/div[@class='product-image-item']//a"))
            {
                if (i < Urunler.Count)
                {
                    if (item.Attributes["href"].Value.Substring(0, 5) != "https")
                        Urunler[i].Link = "https://teknosa.com" + item.Attributes["href"].Value.ToString();
                    else
                        Urunler[i].Link = item.Attributes["href"].Value.ToString();

                    i++;
                }
            }


            return Urunler;
        }
    }
}