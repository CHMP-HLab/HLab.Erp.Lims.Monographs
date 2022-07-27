using System;
using System.Linq;
using System.Windows.Forms;
using HLab.Erp.Core.WebService;
using HLab.Erp.Data;

namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetriever
{

    public class PriceRetrieverHoneywell : PriceRetreiver.PriceRetriever
    {
        string ReferenceUrl => @"http://www.sigmaaldrich.com/catalog/product/(ref)?lang=fr&region=FR";


        Uri GetReferenceUri(string url, string reference)
            => new Uri(url.Replace("(ref)", reference.Split('-')[0].ToLower()));

        bool Navigate()
        {
            Browser.WebBrowser.Navigate(GetReferenceUri(SupplierPrice.Supplier.ReferenceUrl, SupplierPrice.Reference));
                return true;
        }


        public override void Process()
        {
            ChangeUserAgent();
            InternetSetCookie("www.sigmaaldrich.com", "country", "FRA");
            InternetSetCookie("www.sigmaaldrich.com", "cookienotify", "1000");
            InternetSetCookie("www.sigmaaldrich.com", "SialLocaleDef", "CountryCode~FR|WebLang~-2|");

            Browser.WebBrowser.DocumentCompleted += BrowserOnDocumentCompleted;

            Navigate();
        }

//        private readonly string[] _catalogue = {"sial/", "aldrich/", "fluka/"};

void BrowserOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (Browser.WebBrowser.DocumentTitle.Contains("404"))
            {
                if (!Navigate())
                {
                    Browser.WebBrowser.DocumentCompleted -= BrowserOnDocumentCompleted;
                }
                return;
            }


            Browser.WebBrowser.DocumentCompleted -= BrowserOnDocumentCompleted;
            var d = (Browser.WebBrowser?.Document);
            if (d == null) return;

            HtmlElement target = Browser.WebBrowser.Document?.GetElementById("products-list");

            var links = target?.GetElementsByTagName("a");
            if(links!=null)
            foreach (HtmlElement ele in links)
            {
                if (ele.GetAttribute("title") == "View Details")
                {
                    var href = ele.GetAttribute("href");
                    Browser.WebBrowser.DocumentCompleted += Browser_DocumentCompleted;
                    Browser.WebBrowser.Navigate(href);
                }
            }

            target?.AttachEventHandler("onpropertychange", Handler);
        }

async void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Browser.WebBrowser.DocumentCompleted -= Browser_DocumentCompleted;

            RetreiveProperties();

            var target = Browser.WebBrowser.Document?.GetElementById("super-product-table");
            if(target!=null)
            foreach (HtmlElement ele2 in target.GetElementsByTagName("tr"))
            {
               
                var ele3 = ele2.GetElementsByTagName("td");

                if (ele3.Count == 0) continue;

                if (ele3[0].InnerText.Trim() == SupplierPrice.Reference)
                {
                    foreach (var ele5 in ele2.All.Cast<HtmlElement>().Where(ele4 => ele4.GetAttribute("className") == "regular-price"))
                    {
                        var price = await ParsePriceAsync(ele5.InnerText);

                        if (price != null)
                        {
                            SupplierPrice.Cost = price.Item1;
                            SupplierPrice.CurrencyId = price.Item2.Id;
                            return;
                        }
                    }
                    }
            }
        }

async void Handler(object sender, EventArgs eventArgs)
        {
            var target = Browser.WebBrowser.Document?.GetElementById("row"+ SupplierPrice.Reference) ??
                         Browser.WebBrowser.Document?.GetElementById("row" + SupplierPrice.Reference + "-D");

            if (target == null) return;

            foreach (HtmlElement ele2 in target.All)
            {
                if (ele2.GetAttribute("className") != "price" || ele2.TagName != "TD") continue;

                foreach (var ele3 in ele2.All.Cast<HtmlElement>().Where(ele3 => ele3.TagName == "P"))
                {
                    var price = await ParsePriceAsync(ele3.InnerHtml);
                    if (price == null) continue;

                    SupplierPrice.Cost = price.Item1;
                    SupplierPrice.CurrencyId = price.Item2.Id;
                    target?.DetachEventHandler("onpropertychange", Handler);
                    return;
                }
            }
        }

        public PriceRetrieverHoneywell(IDataService dbService, IBrowserService browser) : base(dbService, browser)
        {
        }
    }
}
