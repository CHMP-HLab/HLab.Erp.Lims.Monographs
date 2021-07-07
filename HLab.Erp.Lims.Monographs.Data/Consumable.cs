using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Consumable>;
    public class Consumable : Entity
        , IEntityWithColor
        , IEntityWithIcon
    {
        public Consumable() => H.Initialize(this);

        public string Name
        {
            get => _name.Get(); set => _name.Set(value);
        }
        private readonly IProperty<string> _name = H.Property<string>(c => c.Default(""));


        public string UnitGroup
        {
            get => _unitGroup.Get(); set => _unitGroup.Set(value);
        }

        private readonly IProperty<string> _unitGroup = H.Property<string>(c => c.Default(""));


        public int? SupplierPriceDefaultId
        {
            get => _supplierPriceDefault.Id.Get();
            set => _supplierPriceDefault.Id.Set(value);
        }

        [Ignore]
        public SupplierPrice SupplierPriceDefault
        {
            get => _supplierPriceDefault.Get();
            set => SupplierPriceDefaultId = value.Id;
        }
        private readonly IForeign<SupplierPrice> _supplierPriceDefault = H.Foreign<SupplierPrice>();


        public int? TypeId
        {
            get => _type.Id.Get();
            set => _type.Id.Set(value);
        }

        [Ignore]
        public ConsumableType Type
        {
            get => _type.Get();
            set => TypeId = value.Id;
        }
        private readonly IForeign<ConsumableType> _type = H.Foreign<ConsumableType>();


        public string CasNumber
        {
            get => _casNumber.Get();
            set => _casNumber.Set(value);
        }

        private readonly IProperty<string> _casNumber = H.Property<string>(c => c.Default(""));

        public double? MolarMass
        {
            get => _molarMass.Get(); set => _molarMass.Set(value);
        }

        private readonly IProperty<double?> _molarMass = H.Property<double?>(c => c.Default((double?)default));


        public double? Density
        {
            get => _density.Get(); set => _density.Set(value);
        }

        private readonly IProperty<double?> _density = H.Property<double?>(c => c.Default((double?)default));

        [Ignore] 
        public string IconPath => _iconPath.Get();

        private readonly IProperty<string> _iconPath = H.Property<string>(c => c
            .NotNull(e => e.Type)
            .Set(e => e.Type.IconPath)
            .On(e => e.Type.IconPath)
            .Update()
        );

        [Ignore]
        public int? Color => _color.Get();

        readonly IProperty<int?> _color = H.Property<int?>(c => c
            .On(e => e.Type.Color)
            .Set(e => (int?)e.Type.Color)
        );

    }
}
