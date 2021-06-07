using HLab.Erp.Core.WebService;
using HLab.Erp.Lims.Monographs.Data;

namespace HLab.Erp.Lims.Monographs.Module
{
    //[Export(typeof(IErpService))][Singleton]
    public class LimsMonographieService //: IErpService
    {

        private readonly IBrowserService _browser;
        private readonly ICurrencyService _currency;

        public LimsMonographieService(ICurrencyService currency, IBrowserService browser)
        {
            _currency = currency;
            _browser = browser;
        }

        public void Register()
        {
            //BrowserService.Register(app);

            _currency.Update();
        }
    }
}
