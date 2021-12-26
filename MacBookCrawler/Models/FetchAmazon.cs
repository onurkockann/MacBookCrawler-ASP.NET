using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MacBookCrawler.Models
{
    public class FetchAmazon
    {
        public List<Product> FetchProducts()
        {
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("https://www.amazon.com.tr/s?k=macbook&i=computers&rh=n%3A12601898031%2Cp_89%3AApple&dc&__mk_tr_TR=%C3%85M%C3%85%C5%BD%C3%95%C3%91&qid=1636716537&rnid=13493765031&ref=sr_nr_p_89_1");

            List<Product> Urunler = new List<Product>();

            //1-Getting Product Title.
            int i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//span[@class='a-size-base-plus a-color-base a-text-normal']"))
            {
                Urunler.Add(new Product());
                Urunler[i].Name = item.InnerText.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "/").Replace("&quot;", "Inc").Replace("&#x27;","'").Replace("&#34;","Inc");
                Urunler[i].Firm = "Amazon";//Firm name giving manually
                i++;
            }

            //2-Getting Price Value
            i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//span[@class='a-price-whole']"))
            {
                if (i < Urunler.Count)
                {
                    Urunler[i].Price = item.InnerText.Substring(0, item.InnerText.Length - 1);
                    i++;
                }
            }

            //3-Getting Direct Img Link
            if (doc.DocumentNode.SelectNodes("//img[@class='s-image']") != null)
            {
                i = 0;
                foreach (var item in doc.DocumentNode.SelectNodes("//img[@class='s-image']"))
                {
                    if (i < Urunler.Count)
                    {
                        Urunler[i].ImageURL = item.Attributes["src"].Value.ToString();
                        i++;
                    }
                } 
            }

            //4-Getting Direct Link
            i = 0;
            foreach (var item in doc.DocumentNode.SelectNodes("//*[@id='search']//h2//a[contains(@class, 'a-link-normal')]"))
            {
                if (item.Attributes["href"].Value.Substring(0, 5) != "https")
                    Urunler[i].Link = "https://amazon.com.tr" + item.Attributes["href"].Value.ToString();
                else
                    Urunler[i].Link = item.Attributes["href"].Value.ToString();

                i++;
            }

            return Urunler;
        }
    }
}