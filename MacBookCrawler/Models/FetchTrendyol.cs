using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MacBookCrawler.Models
{
    public class FetchTrendyol
    {
        public List<Product> FetchProducts()
        {
            List<Product> Urunler = new List<Product>();

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.trendyol.com/apple-laptop-x-b101470-c103108");


            //Getting second URL that contains product detail page
            int i = 0;
            string secondURLPath;
            foreach (var item in doc.DocumentNode.SelectNodes("//*[@id='search-app']//div[@class='p-card-chldrn-cntnr']//a"))
            {
                Urunler.Add(new Product());

                secondURLPath = item.Attributes["href"].Value.ToString();
                //1-Getting Direct Link
                if (item.Attributes["href"].Value.Substring(0, 5) != "https")
                    Urunler[i].Link = "https://www.trendyol.com" + secondURLPath;
                else
                    Urunler[i].Link = secondURLPath;

                var newNode = web.Load(Urunler[i].Link);

                //2-Getting Product Title With New XPATH Node
                Urunler[i].Name = newNode.DocumentNode.SelectSingleNode("//*[@id='product-detail-app']//h1[@class='pr-new-br']/span").InnerText;
                Urunler[i].Name = Urunler[i].Name.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "/").Replace("&quot;", "Inc").Replace("&#x27;", "'").Replace("&#34;", "Inc");


                Urunler[i].Firm = "Trendyol";//Firm name giving manually

                //3-Getting Direct Img Link
                if(newNode.DocumentNode.SelectSingleNode("//*[@id='product-detail-app']/div/div/div/div/div/div/div//img") != null)
                    Urunler[i].ImageURL = newNode.DocumentNode.SelectSingleNode("//*[@id='product-detail-app']/div/div/div/div/div/div/div//img").Attributes["src"].Value.ToString();


                i++;
            }

            //4-Getting Price Value
            i = 0;
            foreach (var itemP in doc.DocumentNode.SelectNodes("//div[contains(@class, 'prc-box-sllng')]"))
            {
                Urunler[i].Price = itemP.InnerText.Replace("TL", null);
                i++;
            }


            return Urunler;
        }
    }
}