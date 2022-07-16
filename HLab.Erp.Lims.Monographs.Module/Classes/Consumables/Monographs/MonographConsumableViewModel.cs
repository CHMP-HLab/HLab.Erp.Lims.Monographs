using System.Linq;
using System.Windows.Input;
using HLab.Erp.Base.Data;
using HLab.Erp.Core.WebService;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.Detail;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Monographs
{
    using H = H<MonographConsumableViewModel>;

    public class MonographConsumableViewModel : ViewModel<MonographConsumable>, IMvvmContextProvider
    {
        private readonly IDataService _db;
        private readonly IBrowserService _browser;
        private readonly IUnitService _units;

        public MonographConsumableViewModel(IDataService db, IBrowserService browser, IUnitService units)
        {
            _db = db;
            _browser = browser;
            _units = units;
            H.Initialize(this);
        }

        public ObservableQuery<SupplierPrice> Prices
        {
            get => _prices.Get();
            set => _prices.Set(value.AddFilter(tf => tf.ConsumableId == Model.ConsumableId)
                .AddOnCreate(h =>
                {
                    var c = Model?.Consumable;
                    if (c == null || Supplier == null)
                    {
                        h.Done = false;
                        return;
                    }

                    var tf = h.Entity;
                    tf.SupplierId = Supplier.Id;
                    tf.ConsumableId = c.Id;
                    tf.CurrencyId = Supplier.CurrencyId;

                    var unit = _units.Default(c.UnitGroup);

                    tf.UnitId = unit?.Id;
                    tf.Quantity = unit?.DefaultQty ?? 0;
                    h.Done = true;
                })
                .AddOnDelete(h => { h.Done = true; })
                .FluentUpdate());
        }

        private readonly IProperty<ObservableQuery<SupplierPrice>> _prices = H.Property<ObservableQuery<SupplierPrice>>(c => c
            .On(e => e.Model.ConsumableId)
            .Do(e => e.Prices.OnTriggered())
        );

        public ObservableViewModelCollection<SupplierPriceViewModel> PricesViewModel
        {
            get => _pricesViewModel.Get();
            set => _pricesViewModel.Set(value.SetViewModeContext(() => MvvmContext)
                .SetViewMode<ViewModeList>()
                .Link(() => Prices));
        }

        private readonly IProperty<ObservableViewModelCollection<SupplierPriceViewModel>> _pricesViewModel = H.Property<ObservableViewModelCollection<SupplierPriceViewModel>>(c => c
            .On(e => e.Prices)
            .Do(e => e.PricesViewModel.OnTriggered())
        );

        public ICommand AddCommand { get; } = H.Command(c => c
            .CanExecute( e => e.Supplier != null && e.Model?.Consumable != null)
            .Action(
                (e) =>
                {
                    e._db.Add<SupplierPrice>(
                        tf =>
                        {
                            var cs = e.Model?.Consumable;
                            if (cs == null || e.Supplier == null) return;

                            tf.SupplierId = e.Supplier.Id;
                            tf.ConsumableId = cs.Id;
                            tf.CurrencyId = e.Supplier.CurrencyId;

                            var unit = e._units.Default(cs.UnitGroup);

                            tf.UnitId = unit?.Id;
                            tf.Quantity = unit?.DefaultQty ?? 0;
                        },
                        tf =>
                        {
                            e.Prices.FluentUpdate();
                            e.Prices.Selected = tf;
                            if (e.Prices.Count == 1 || e.Model.SupplierPriceId == null)
                                e.Model.SupplierPriceId = tf.Id;
                        }


                    );
                })
            .On(e => e.Supplier)
            .On(e => e.Model.Consumable)
            .CheckCanExecute()
        );

        [TriggerOn(nameof(PricesViewModel), "Selected")]
        public void UpdateSelected()
        {
            Prices.Selected = PricesViewModel.Selected.Model;
        }

        public ICommand RemoveCommand { get; } = H.Command(c => c
            .CanExecute(e => e.Prices.Selected != null)
            .Action(
                (e) =>
                {
                    e._db.Delete<SupplierPrice>(
                        e.Prices.Selected,

                        tf =>
                        {
                            e.Prices.Selected = null;
                            e.Prices.FluentUpdate();
                            if (e.Prices.Count == 1 || e.Model.SupplierPriceId == null)
                                e.Model.SupplierPriceId = e.Prices.FirstOrDefault()?.Id;
                        });
                }
                )
            .On(e => e.Prices.Selected)
            .CheckCanExecute()
        );



        public Supplier Supplier
        {
            get => _supplier.Get();
            set => _supplier.Set(value);
        }

        private readonly IProperty<Supplier> _supplier = H.Property<Supplier>(c => c.Default((Supplier)default));


        public ObservableQuery<Supplier> SupplierList
        {
            get => _supplierList.Get();
            set => _supplierList.Set(value.FluentUpdate());
        }

        private readonly IProperty<ObservableQuery<Supplier>> _supplierList = H.Property<ObservableQuery<Supplier>>(c => c
            .On(e => e)
            .Do(e => e.SupplierList.Update())
        );

        public ICommand Command { get; } = H.Command(c => c
            .CanExecute(e => !string.IsNullOrWhiteSpace(e.Supplier?.Url))
            .Action(
                (e, n) =>
                {
                    if (e.Supplier == null) return;

                    var cs = e.Model?.Consumable;
                    var url = (c == null) ? e.Supplier.Url : e.Supplier.SearchUrl?.Replace("(designation)", cs?.Name);

                    e._browser.Navigate(url);
                }
                )
            .On(e => e.Supplier).CheckCanExecute()
        );


        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
                ctx.AddCreator<SupplierPriceViewModel>(e => e.MonographConsumable = Model);
            
        }
    }

}
