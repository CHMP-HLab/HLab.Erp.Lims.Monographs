﻿using HLab.Erp.Core.WebService;
using HLab.Erp.Lims.Monographs.Data;

namespace HLab.Erp.Lims.Monographs.Module
{
    public class LimsMonographService //: IErpService
    {
        readonly IBrowserService _browser;
        readonly ICurrencyService _currency;

        public LimsMonographService(ICurrencyService currency, IBrowserService browser)
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
