using System;
using System.Windows.Forms;
using HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetreiver;

namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetriever
{
    public class PriceRetrieverEdqm : PriceRetreiver.PriceRetriever
    {
        public override void Process()
        {
            Browser.WebBrowser.DocumentCompleted += BrowserOnLoadCompleted;

            var url = new Uri(SupplierPrice.Supplier.ReferenceUrl.Replace("(ref)", SupplierPrice.Reference.Split('-')[0].ToLower()));

            Browser.WebBrowser.Navigate(url);
        }

        private string GetValue(string name)
        {
            var tr = Document.GetElementsByTagName("tr");

            foreach (HtmlElement element in tr)
            {
                var td = element.GetElementsByTagName("td");
                if (td.Count < 2) continue;

                var font = td[0].GetElementsByTagName("font");
                if (font.Count <= 0) continue;

                if (!font[0].InnerHtml.Contains(name)) continue;

                var font2 = td[1].GetElementsByTagName("font");

                var s = "";
                for(var i = 0; i<font2.Count; i++)
                {
                    s += font2[i].InnerHtml;
                }

                return s?.Trim();
            }

            return null;
        }

        private async void BrowserOnLoadCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Browser.WebBrowser.DocumentCompleted -= BrowserOnLoadCompleted;
            var d = (Browser.WebBrowser?.Document);
            if (d == null) return;

                var price = await ParsePriceAsync(GetValue("Price"));
                var cas = GetValue("CAS Registry Number");
                var qty = await ParseQtyAsync(GetValue("Unit quantity"));

            if (qty != null && SupplierPrice.UnitId != null && qty.Item2.Id != SupplierPrice.UnitId)
            {
                SupplierPrice.Message = "Unité inconsistante : " + qty.Item1 + " " + qty.Item2.Symbol;
                return;
            }

            if (qty != null && Math.Abs(SupplierPrice.Quantity) > double.Epsilon &&
                Math.Abs(qty.Item1 - SupplierPrice.Quantity) > double.Epsilon)
            {
                SupplierPrice.Message = "Quantité unitaire inconsistante : " + qty.Item1 + " " + qty.Item2.Symbol;
                return;
            }

            if (!string.IsNullOrWhiteSpace(cas) && !string.IsNullOrWhiteSpace(SupplierPrice.Consumable.CasNumber) &&
                cas != SupplierPrice.Consumable.CasNumber)
            {
                SupplierPrice.Message = "CAS inconsistant : " + cas;
                return;
            }

            if (price != null)
            {
                SupplierPrice.Cost = price.Item1;
                SupplierPrice.CurrencyId = price.Item2.Id;
                SupplierPrice.Message = "Ok";
                SupplierPrice.DateValid = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(cas))
            {
                SupplierPrice.Consumable.CasNumber = cas;
            }
            if (qty != null)
            {
                SupplierPrice.Quantity = qty.Item1;
                SupplierPrice.UnitId = qty.Item2.Id;
            }
        }

    }
}
