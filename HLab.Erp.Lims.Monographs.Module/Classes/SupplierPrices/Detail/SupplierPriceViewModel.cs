using System;
using System.Linq;
using System.Windows.Input;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.PriceRetreiver;
using HLab.Mvvm;

using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.Detail
{
    using H = H<SupplierPriceViewModel>;

    public class SupplierPriceViewModel : ViewModel<SupplierPrice>
    {
        readonly IDataService _db;

        public SupplierPriceViewModel(IDataService db)
        {
            _db = db;
            H.Initialize(this);
        }

        public MonographConsumable MonographConsumable
        {
            get => _monographConsumable.Get();
            set => _monographConsumable.Set(value);
        }

        readonly IProperty<MonographConsumable> _monographConsumable = H.Property<MonographConsumable>();


        public ObservableQuery<Unit> UnitList
        {
            get => _unitList.Get();
            set => _unitList.Set(value.AddFilter(e => e.UnitClass.Symbol == Model.Consumable.UnitGroup));
        }

        readonly IProperty<ObservableQuery<Unit>> _unitList = H.Property<ObservableQuery<Unit>>(c => c
            .On(e => e.Model.Consumable.UnitGroup)
            .Do(e => e.UnitList.OnTriggered())
        );



        [TriggerOn(nameof(Model), "UnitId")]
        public Unit Unit
        {
            get => UnitList.FirstOrDefault(e => e.Id == Model.UnitId);
            set => Model.UnitId = value?.Id;
        }

        public ObservableQuery<Currency> CurrencyList
        {
            get => _currencyList.Get();
            set => _currencyList.Set(value.FluentUpdate());
        }

        readonly IProperty<ObservableQuery<Currency>> _currencyList = H.Property<ObservableQuery<Currency>>();



        [TriggerOn(nameof(Model), "Currency")]
       public Currency Currency
        {
            get => CurrencyList.FirstOrDefault(e => e.Id == Model.CurrencyId);
            set => Model.CurrencyId = value?.Id;
        }



        public bool IsSelected
        {
            get => _isSelected.Get();
            set => MonographConsumable.SupplierPriceId = (value && !DefaultSelected) ? (int?)Model.Id : null;
        }

        readonly IProperty<bool> _isSelected = H.Property<bool>(c => c
            .On(e => e.DefaultSelected)
            .On(e => e.MonographConsumable.SupplierPriceId)
            .Set(e => (e.MonographConsumable.SupplierPriceId == null)
                ? e.DefaultSelected
                : e.Model.Id == e.MonographConsumable.SupplierPriceId));


        public bool SelectedEnabled => _selectedEnabled.Get();

        readonly IProperty<bool> _selectedEnabled = H.Property<bool>(c => c
            .On(e => e.IsSelected)
            .On(e => e.DefaultSelected)
            .Set(e => !(e.IsSelected && e.DefaultSelected)));




        public bool DefaultSelected
        {
            get => _defaultSelected.Get();
            set
            {
                if (!value) return;

                MonographConsumable.Consumable.SupplierPriceDefault = Model;

                if (MonographConsumable.SupplierPriceId == Model.Id)
                {
                    MonographConsumable.SupplierPrice = null;
                }
            }                 
        }

        readonly IProperty<bool> _defaultSelected = H.Property<bool>(c => c
            .On(e => e.MonographConsumable.SupplierPrice)
            .On(e => e.MonographConsumable.Consumable.SupplierPriceDefaultId)
            .Set(e => e.Model.Id == e.MonographConsumable.Consumable?.SupplierPriceDefaultId)
        );

 
        public double UnitPriceView => _unitPriceView.Get();

        readonly IProperty<double> _unitPriceView = H.Property<double>(c => c
            .On(e => e.Model.UnitPrice)
            .Set(e => e.Model.UnitPrice)
        );


        public Func<string,IPriceRetriever> GetPriceRetreiver { get;}


        public ICommand UpdateCommand { get; } = H.Command(c => c
            .Action(
                (self) =>
                {
                    var name = self.Model.Supplier?.PriceRetriever;

                    if (string.IsNullOrEmpty(name)) return;

                    try
                    {
                        var priceRetriever = self.GetPriceRetreiver(name);

                        //var parser = Activator.CreateInstance("HLab.Erp.Lims.Monographs.Module", "HLab.Erp.Lims.Monographs.Module" + "._SupplierPrices.PriceRetreiver.PriceRetriever" + tarif.Supplier.PriceRetriever).Unwrap();

                        //var priceRetriever = parser as PriceRetriever;
                        priceRetriever?.Process(self.Model);
                    }
                    catch (Exception e)
                    {
                        (self.UpdateCommand as NCommand)?.CanExecute(false);
                        //UpdateCommand.SetCanExecute(false);
                    }
                })
        );

    }
}
