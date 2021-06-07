namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetriever
{
    // TODO : Port to cefsharp

    /*
        [Export(typeof(IPriceRetriever))]
        public class PriceRetrieverSigma //: PriceRetriever
        {

            private string ReferenceUrl => @"http://www.sigmaaldrich.com/catalog/product/(cat)/(ref)?lang=fr&region=FR";


            private Uri GetReferenceUri(string url, string reference)
                => new Uri(url.Replace("(ref)", reference.Split('-')[0].ToLower()));

            private bool Navigate()
            {
                string[] tf = SupplierPrice.Reference.Split('-');

                string reference = tf[0].ToLower();

                // hack pour corrigé 30620-1KG-M non trouvé
                if (tf[tf.Length - 1].ToLower() == "m") reference += "m";

                var url1 = ReferenceUrl.Replace("(ref)", reference);

                _current++;
                if (_current < _catalogue.Length)
                {
                    var url = url1.Replace("(cat)", _catalogue[_current]);

                    Browser.Navigate(url);
                    return true;
                }
                return false;
            }


            public override void Process()
            {
                ChangeUserAgent();
                InternetSetCookie("www.sigmaaldrich.com", "country", "FRA");
                InternetSetCookie("www.sigmaaldrich.com", "cookienotify", "1000");
                InternetSetCookie("www.sigmaaldrich.com", "SialLocaleDef", "CountryCode~FR|WebLang~-2|");
                InternetSetCookie("www.sigmaaldrich.com", "Cck", "present");

                Browser.WebBrowser.DocumentCompleted += BrowserOnDocumentCompleted;

                Navigate();
            }

            private readonly string[] _catalogue = {"sial", "aldrich", "fluka", "mm"};
            private int _current = -1;

            private void BrowserOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
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

                var d = (mshtml.IHTMLDocument3)(Browser.WebBrowser?.Document?.DomDocument);
                if (d == null) return;

                var target = (mshtml.IHTMLElement2)d.getElementById("pricingContainerMessage");

                //target?.AttachEventHandler("onpropertychange", Handler);

                var events = (mshtml.HTMLElementEvents2_Event)target;

                if(events!=null)
                    events.onpropertychange += Events_onpropertychange; ;
            }

            private void Events_onpropertychange(mshtml.IHTMLEventObj pEvtObj)
            {
                RetreiveProperties();

                var target = Browser.WebBrowser.Document?.GetElementById("row" + SupplierPrice.Reference) ??
                             Browser.WebBrowser.Document?.GetElementById("row" + SupplierPrice.Reference + "-D");

                if (target == null) return;

                foreach (HtmlElement ele2 in target.GetElementsByTagName("td"))
                {
                    if (ele2.GetAttribute("className") != "price") continue;

                    foreach (HtmlElement ele3 in ele2.GetElementsByTagName("p"))
                    {
                        var price = ParsePrice(ele3.InnerHtml);
                        if (price == null) continue;

                        SupplierPrice.Cost = price.Item1;
                        SupplierPrice.CurrencyId = price.Item2.Id;
                        target?.DetachEventHandler("onpropertychange", Handler);
                        return;
                    }
                }
            }

            private void Handler(object sender, EventArgs eventArgs)
            {
                RetreiveProperties();

                var target = Browser.WebBrowser.Document?.GetElementById("row"+ SupplierPrice.Reference) ??
                             Browser.WebBrowser.Document?.GetElementById("row" + SupplierPrice.Reference + "-D");

                if (target == null) return;

                foreach (HtmlElement ele2 in target.GetElementsByTagName("td"))
                {
                    if (ele2.GetAttribute("className") != "price") continue;

                    foreach (HtmlElement ele3 in ele2.GetElementsByTagName("p"))
                    {
                            var price = ParsePrice(ele3.InnerHtml);
                            if (price == null) continue;

                            SupplierPrice.Cost = price.Item1;
                            SupplierPrice.CurrencyId = price.Item2.Id;
                            target?.DetachEventHandler("onpropertychange", Handler);
                            return;
                    }
                }
            }
        }
        */
}
