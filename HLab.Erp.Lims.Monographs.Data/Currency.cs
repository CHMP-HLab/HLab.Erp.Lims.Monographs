using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{

    using H = HD<Currency>;

    public partial class Currency : Entity
    {
        public Currency() => H.Initialize(this);

        public string Name
        {
            get => _name.Get(); set => _name.Set(value);
        }

        readonly IProperty<string> _name = H.Property<string>(c => c.Default(""));

        public string Symbol
        {
            get => _symbol.Get(); set => _symbol.Set(value);
        }

        readonly IProperty<string> _symbol = H.Property<string>(c => c.Default(""));

        public string Iso
        {
            get => _iso.Get(); set => _iso.Set(value);
        }

        readonly IProperty<string> _iso = H.Property<string>(c => c.Default(""));

        public decimal Rate
        {
            get => _rate.Get(); set => _rate.Set(value);
        }

        readonly IProperty<decimal> _rate = H.Property<decimal>(c => c.Default(decimal.One));

        [Timestamp]
        public DateTime? Date
        {
            get => _date.Get(); set => _date.Set(value);
        }

        readonly IProperty<DateTime?> _date = H.Property<DateTime?>(c => c.Default((DateTime?)null));



    }

    public partial class SupplierPrice
    {
        public int? CurrencyId
        {
            get => _currency.Id.Get();
            set => _currency.Id.Set(value);
        }

        [Ignore]
        public Currency Currency
        {
            get => _currency.Get();
            set => CurrencyId = value.Id;
        }

        readonly IForeign<Currency> _currency = HD<SupplierPrice>.Foreign<Currency>();


        [Ignore]
        [TriggerOn(nameof(Cost))]
        [TriggerOn(nameof(Currency), "Rate")]
        public decimal CostEuro => Cost / (Currency?.Rate ?? 1);

    }

    public partial class Supplier : Entity
    {
        public int? CurrencyId
        {
            get => _currencyId.Get();
            set => _currencyId.Set(value);
        }

        readonly IProperty<int?> _currencyId = HD<Supplier>.Property<int?>();

        [NotMapped]
        public Currency Currency
        {
            get => _currency.Get();
            set => CurrencyId = value.Id;
        }

        readonly IProperty<Currency> _currency = HD<Supplier>.Property<Currency>(c => c.Foreign(e => e.CurrencyId));

    }
}
