using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using HLab.Erp.Core.WebService;
using HLab.Erp.Data;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Units;


//using Erp.Data;

namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetreiver
{
    public interface IPriceRetriever
    {
        void Process(SupplierPrice tarif);
    }

    public abstract class PriceRetriever : IPriceRetriever
    {
        protected IDataService DbService { get; private set; }
        protected IBrowserService Browser { get; private set; }

        public void Inject(IDataService dbService, IBrowserService browser)
        {
            DbService = dbService;
            Browser = browser;
        }

        protected static object LockBrowser = new object();

        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(
            int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        const int URLMON_OPTION_USERAGENT = 0x10000001;
        const int URLMON_OPTION_USERAGENT_REFRESH = 0x10000002;

        public void ChangeUserAgent()
        {
            string ua = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0";

            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT_REFRESH, null, 0, 0);
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, ua, ua.Length, 0);
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        protected SupplierPrice SupplierPrice;


        protected HtmlDocument Document => Browser.WebBrowser.Document as HtmlDocument;

        public void Process(SupplierPrice tarif)
        {
            SupplierPrice = tarif;
            Process();
        }

        public bool SetCasNumber(string value)
        {
            SupplierPrice.Consumable.CasNumber = value;

            return true;
        }

        public bool SetAssay(string value, CultureInfo culture = null)
        {
                value = value.Replace("≥", "");

                double ratio = 1;

                if (value.Contains("%"))
                {
                    ratio = 100.0;
                    value = value.Replace("%", "");
                }

                value = value.Split(' ')[0];

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var assay))
                    return false;

                assay /= ratio;

                SupplierPrice.Assay = assay;
//                ctx.SaveChanges();
                return true;
            
        }

        public bool SetDensity(string value, CultureInfo culture = null)
        {
                double ratio = 1;

                if (value.Contains("g/cm3"))
                {
                    ratio = 1;
                    value = value.Replace("g/cm3", "");
                }

                if (value.Contains("g/ml"))
                {
                    ratio = 1;
                    value = value.Replace("g/ml", "");
                }

                value = value.Split(' ')[0];

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var density))
                    return false;

                SupplierPrice.Density = density * ratio;
                return true;
        }

        public void RetreiveProperties()
        {
            foreach (HtmlElement t1 in Browser.WebBrowser.Document?.GetElementsByTagName("table"))
            {
                //var t1 = Browser.Document?.GetElementById("product-attribute-specs-table");
                if (t1 != null)
                    foreach (HtmlElement ele2 in t1.GetElementsByTagName("tr"))
                    {
                        var ele3 = ele2.GetElementsByTagName("td");
                        if (ele3.Count == 0) continue;

                        string colname = ele3[0].InnerText.ToLower();
                        while (colname.Contains("&nbsp;")) colname = colname.Replace("&nbsp;", " ");

                        colname = colname.Trim();

                        if (ele3[0].InnerText.Trim() == "assay")
                        {
                            SetAssay(ele3[1].InnerText.Trim());
                        }
                        else if (ele3[0].InnerText.Trim() == "density")
                        {
                            SetDensity(ele3[1].InnerText.Trim());
                        }
                    }
            }
        }

        public async Task<Tuple<double,Unit>> ParseQtyAsync(string value, CultureInfo culture = null)
        {
            if (value == null) return null;
            var values = value.Split(' ');

            if (values.Length == 2)
            {
                if (double.TryParse(values[0], out var qty))
                {
                    var unit = await DbService.FetchAsync<Unit>().FirstOrDefaultAsync(u => values[1] == u.Symbol);
                    if (unit != null)
                    {
                        return new Tuple<double, Unit>(qty,unit);
                    }
                }
            }

            return null;
        }

        public async Task<Tuple<decimal,Currency>> ParsePriceAsync(string value, CultureInfo culture = null)
        {
            if (value == null) return null;

            Currency currency = SupplierPrice.Supplier.Currency;

            if (culture == null) culture = CultureInfo.InvariantCulture;

            //TODO use currency service
            
                await foreach (var m in DbService.FetchAsync<Currency>())
                {
                    if (value.Contains(m.Symbol))
                    {
                        currency = m;
                        value = value.Replace(m.Symbol, "");
                    }
                    if (value.Contains(m.Iso))
                    {
                        currency = m;
                        value = value.Replace(m.Iso, "");
                    }
                }

            if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var tarif))
                    return null;

                tarif = (decimal) ((float)tarif * (1 + SupplierPrice.Supplier.Tax));

            return new Tuple<decimal, Currency>(tarif,currency);
        }

        public abstract void Process();
    }

    public class PriceRetreiverExeption : Exception
    {
        public PriceRetreiverExeption(string message) : base(message)
        {
        }
    }
}
