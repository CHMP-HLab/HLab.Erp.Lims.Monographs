﻿using System.Windows.Input;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Currencies
{
    public class CurrenciesToolViewModel : ViewModel
    {
        public class Bootloader : NestedBootloader
        {
            readonly IAclService _acl;

            public Bootloader(IAclService acl)
            {
                _acl = acl;
            }
            public override bool Allowed => _acl.IsGranted(AclRights.BetaTest);
            public override string MenuPath => "tools";
        }

        readonly ICurrencyService _currency;

        public CurrenciesToolViewModel(ICurrencyService currency)
        {
            _currency = currency;

            H<CurrenciesToolViewModel>.Initialize(this);
        }

        public string Title => "{Currencies}";

        public ObservableQuery<Currency> CurrenciesList
        {
            get => _currenciesList.Get();
            set => _currenciesList.Set(value.FluentUpdate());
        }

        readonly IProperty<ObservableQuery<Currency>> _currenciesList = H<CurrenciesToolViewModel>.Property<ObservableQuery<Currency>>(c => c
        );



        public ICommand UpdateCommand { get; } = H<CurrenciesToolViewModel>.Command(c => c
            .Action(
                (e) => e._currency.Update())
        );

    }
}
