using HLab.Erp.Data;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<MonographConsumable>;
    public partial class MonographConsumable : Entity, IOrderedEntity
    {
        public MonographConsumable() => H.Initialize(this);

        public int? MonographId
        {
            get => _monograph.Id.Get();
            set => _monograph.Id.Set(value);
        }

        [Ignore]
        public Monograph Monograph
        {
            get => _monograph.Get();
            set => _monograph.Set(value);
        }
        private readonly IForeign<Monograph> _monograph = H.Foreign<Monograph>(); 


        public int? ConsumableId
        {
            get => _consumable.Id.Get();
            set => _consumable.Id.Set(value);
        }

        [Ignore]
        public Consumable Consumable
        {
            get => _consumable.Get();
            set => _consumable.Set(value);
        }
        private readonly IForeign<Consumable> _consumable = H.Foreign<Consumable>(); 


        public int? SupplierPriceId
        {
            get => _supplierPriceId.Get();
            set => _supplierPriceId.Set(value);
        }

        private readonly IProperty<int?> _supplierPriceId = H.Property<int?>();

        [Ignore]
        public SupplierPrice SupplierPrice
        {
            get => _supplierPrice.Get();
            set => SupplierPriceId = value.Id;
        }

        private readonly IProperty<SupplierPrice> _supplierPrice = H.Property<SupplierPrice>(c => c.Foreign(e => e.SupplierPriceId));


        public int? Order
        {
            get => _order.Get();
            set => _order.Set(value);
        }

        private readonly IProperty<int?> _order = H.Property<int?>(c => c.Default((int?)default));


        public string Designation
        {
            get => _designation.Get();
            set => _designation.Set(value);
        }

        private readonly IProperty<string> _designation = H.Property<string>(c => c.Default(""));


        public string Note
        {
            get => _note.Get();
            set => _note.Set(value);
        }

        private readonly IProperty<string> _note = H.Property<string>(c => c.Default(""));


        [Ignore]
        public string UnitGroup => _unitGroup.Get();

        private  readonly IProperty<string> _unitGroup = H.Property<string>(c => c
            .On(e => e.Consumable.UnitGroup)
            .Set(e=>e.Consumable.UnitGroup)
        );


         [Ignore]
        public string RightUnitGroup => _rightUnitGroup.Get();

        private  readonly IProperty<string> _rightUnitGroup = H.Property<string>(c => c
            .On(e => e.Consumable.UnitGroup)
            .Set(e=>e.Consumable.UnitGroup)
        );
    }
}
