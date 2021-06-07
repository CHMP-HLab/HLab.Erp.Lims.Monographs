using System;
using System.Collections.Generic;
using System.Linq;
using HLab.Erp.Data;
using Newtonsoft.Json;

namespace HLab.Erp.Lims.Monographs.Data
{
    public class CurrencyService : ICurrencyService
    {
        private IDataService _db;

        public void Inject(IDataService db)
        {
            _db = db;
        }

        public async void Update()
        {
            var request =
                $"http://data.fixer.io/api/latest?access_key=86e78966185cf29cadc4ad69d358658c&base=EUR";

            using (var wc = new System.Net.WebClient())
            {
                try
                {
                    var apiResponse = wc.DownloadString(request);    // This is a blocking operation.               

                    //                    var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                    dynamic result = JsonConvert.DeserializeObject(apiResponse);

                    var date = DateTime.Parse(result["date"].ToString());

                    var currencies = await _db.FetchAsync<Currency>().ToListAsync();


                    foreach (var currency in currencies.Where(c => c.Iso != "EUR"))
                    {
                        try
                        {
                            var rate = (decimal)(result["rates"][currency.Iso]);
                            currency.Rate = rate;
                            currency.Date = DateTime.Now;
                        }
                        catch (KeyNotFoundException) { }
                    }
                }
                catch (System.Net.WebException)
                { }
            }
        }

    }
}
