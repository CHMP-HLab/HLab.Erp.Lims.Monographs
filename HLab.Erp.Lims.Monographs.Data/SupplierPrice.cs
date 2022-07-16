using System;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<SupplierPrice>;

    public partial class SupplierPrice : Entity
    {
        public SupplierPrice() => H.Initialize(this);

        public decimal Cost
        {
            get => _cost.Get();
            set => _cost.Set(value);
        }

        private readonly IProperty<decimal> _cost = H.Property<decimal>(c => c.Default(Decimal.Zero));

        public decimal Discount
        {
            get => _discount.Get();
            set => _discount.Set(value);
        }

        private readonly IProperty<decimal> _discount = H.Property<decimal>(c => c.Default(Decimal.Zero));


        public double Quantity
        {
            get => _quantity.Get();
            set => _quantity.Set(value);
        }

        private readonly IProperty<double> _quantity = H.Property<double>(c => c.Default(0.0));


        public string Reference
        {
            get => _reference.Get();
            set => _reference.Set(value);
        }

        private readonly IProperty<string> _reference = H.Property<string>(c => c.Default(""));

        public int? UnitId
        {
            get => _unit.Id.Get();
            set => _unit.Id.Set(value);
        }

        [Ignore]
        public Unit Unit
        {
            get => _unit.Get();
            set => UnitId = value.Id;
        }

        private readonly IForeign<Unit> _unit = H.Foreign<Unit>();


        public int? SupplierId
        {
            get => _supplier.Id.Get();
            set => _supplier.Id.Set(value);
        }

        [Ignore]
        public Supplier Supplier
        {
            get => _supplier.Get();
            set => SupplierId = value.Id;
        }

        private readonly IForeign<Supplier> _supplier = H.Foreign<Supplier>();


         public int? ConsumableId
        {
            get => _consumable.Id.Get();
            set => _consumable.Id.Set(value);
        }

        [Ignore]
        public Consumable Consumable
        {
            get => _consumable.Get();
            set => ConsumableId = value.Id;
        }

        private readonly IForeign<Consumable> _consumable = H.Foreign<Consumable>();


        public double? Assay
        {
            get => _assay.Get();
            set => _assay.Set(value);
        }

        private readonly IProperty<double?> _assay = H.Property<double?>(c => c.Default(default(double?)));


        public double? Density
        {
            get => _density.Get();
            set => _density.Set(value);
        }

        private readonly IProperty<double?> _density = H.Property<double?>(c => c.Default(default(double?)));


        public string Message
        {
            get => _message.Get();
            set => _message.Set(value);
        }

        private readonly IProperty<string> _message = H.Property<string>(c => c.Default(default(string)));


        public DateTime? DateValid
        {
            get => _dateValid.Get();
            set => _dateValid.Set(value);
        }

        private readonly IProperty<DateTime?> _dateValid = H.Property<DateTime?>(c => c.Default((DateTime?)DateTime.MinValue));

    }
}
