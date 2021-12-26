using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MacBookCrawler.Models
{
    public class FetchN11
    {
        public List<Product> FetchProducts()
        {
            List<Product> Urunler = new List<Product>();

            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.n11.com/bilgisayar/dizustu-bilgisayar?m=Apple");


            //Getting second URL that contains product detail page
            int i = 0;
            string secondURLPath;
            foreach (var item in doc.DocumentNode.SelectNodes("//section[@class='group listingGroup resultListGroup import-search-view']//a[@class='plink']"))
            {
                Urunler.Add(new Product());

                secondURLPath = item.Attributes["href"].Value.ToString();
                //1-Getting Direct Link
                if (item.Attributes["href"].Value.Substring(0, 5) != "https")
                    Urunler[i].Link = "https://www.n11.com" + secondURLPath;
                else
                    Urunler[i].Link = secondURLPath;

                var newNode = web.Load(Urunler[i].Link);

                //2-Getting Product Title With New XPATH Node
                Urunler[i].Name = newNode.DocumentNode.SelectSingleNode("//div[@class='unf-p-lBox']/div/div/div/div[@class='nameHolder']//h1[@class='proName']").InnerText;
                Urunler[i].Name = Urunler[i].Name.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "/").Replace("&quot;", "Inc").Replace("&#x27;", "'").Replace("&#34;", "Inc");


                Urunler[i].Firm = "N11";//Firm name giving manually

                //3-Getting Direct Img Link
                if(newNode.DocumentNode.SelectSingleNode("//div[@class='unf-p-lBox']/div/div/div[@class='imgObj']//a") != null)
                    Urunler[i].ImageURL = newNode.DocumentNode.SelectSingleNode("//div[@class='unf-p-lBox']/div/div/div[@class='imgObj']//a").Attributes["href"].Value.ToString();


                i++;
            }

            //4-Getting Price Value
            i = 0;
            foreach (var itemP in doc.DocumentNode.SelectNodes("//div[@class='proDetail']/span//ins"))
            {
                Urunler[i].Price = itemP.InnerText.Replace("TL", null);
                i++;
            }

            return Urunler;
        }
    }
}