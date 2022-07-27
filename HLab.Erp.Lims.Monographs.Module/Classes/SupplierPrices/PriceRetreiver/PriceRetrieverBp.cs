using HLab.Erp.Core.WebService;
using HLab.Erp.Data;
using System;
using System.Windows.Forms;

namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetreiver
{

    public class PriceRetrieverBp : PriceRetriever
    {
        public PriceRetrieverBp(IDataService dbService, IBrowserService browser) : base(dbService, browser)
        {
        }

        public override void Process()
        {
            //ChangeUserAgent();

            InternetSetCookie("www.sigmaaldrich.com", "country", "FRA");
            InternetSetCookie("www.sigmaaldrich.com", "cookienotify", "1000");
            InternetSetCookie("www.sigmaaldrich.com", "SialLocaleDef", "CountryCode~FR|WebLang~-2|");

            Browser.WebBrowser.DocumentCompleted += BrowserOnDocumentCompleted;
            var url = new Uri(SupplierPrice.Supplier.ReferenceUrl.Replace("(ref)", SupplierPrice.Reference.Split('-')[0].ToLower()));
            Browser.WebBrowser.Navigate(url);
        }

        void BrowserOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            Browser.WebBrowser.DocumentCompleted -= BrowserOnDocumentCompleted;

            var doc = (Browser.WebBrowser?.Document);

            var target = doc?.GetElementsByTagName("table");

            if (target?.Count > 0)
            {
                target = target[0].GetElementsByTagName("a");
                if (target.Count > 0)
                {
                    var url = target[0].GetAttribute("href");
                    Browser.WebBrowser.DocumentCompleted += BrowserOnDocumentCompleted2;
                    Browser.WebBrowser.Navigate(url);
                }
            }
        }

        async void BrowserOnDocumentCompleted2(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            Browser.WebBrowser.DocumentCompleted -= BrowserOnDocumentCompleted2;
            var doc = (Browser.WebBrowser?.Document);
            if (doc == null) return;

            var target = doc.GetElementsByTagName("th");

            foreach (HtmlElement t in target)
            {
                if (t.InnerHtml != "Price:") continue;
                var t2 = t.Parent?.GetElementsByTagName("td");
                if (!(t2?.Count > 0)) continue;

                var price = await ParsePriceAsync(t2[0].InnerHtml);

                if (price!=null)
                {
                    SupplierPrice.Cost = price.Item1;
                    SupplierPrice.CurrencyId = price.Item2.Id;
                    return;
                }
            }

        }

    }
}
