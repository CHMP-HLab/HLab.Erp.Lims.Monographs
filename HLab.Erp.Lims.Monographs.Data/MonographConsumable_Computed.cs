using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<MonographConsumable>;
    public partial class MonographConsumable
    {
        //public override IObservableFilter<MonographLink> LeftLinks { get; } =
        //    H.Filter<MonographLink>((e, f) => f.AddFilter(i => false).Link(() => e.Monograph?.Links));


        ////TODO : could just be empty collection not linked
        //public override IObservableFilter<MonographLink> RightLinks { get; } =
        //    H.Filter<MonographLink>((e, f) => f.AddFilter(i => i.MonographConsumableId == e.Id).Link(() => e.Monograph?.Links));

        [NotMapped]
        public SupplierPrice ActualSupplierPrice => _actualSupplierPrice.Get();

        private readonly IProperty<SupplierPrice> _actualSupplierPrice = H.Property<SupplierPrice>(c => c
            .On(e => e.SupplierPrice)
            .On(e => e.Consumable.SupplierPriceDefault)
            .Set(e => e.SupplierPrice ?? e.Consumable?.SupplierPriceDefault)
        );

        [NotMapped]
        public double UnitPrice => _unitPrice.Get();

        private readonly IProperty<double> _unitPrice = H.Property<double>(c => c
            .On(e => e.ActualSupplierPrice.UnitPrice)
            .Set(e => e.ActualSupplierPrice?.UnitPrice??0)
        );

        [NotMapped]
        public double Cost => _cost.Get();
        private readonly IProperty<double> _cost = H.Property<double>(c => c
            .On(e => e.UnitPrice)
            .On(e => e.QtyAbs)
            .Set(e => e.QtyAbs * e.UnitPrice)
        );

        [NotMapped]
        public double CostNext => _costNext.Get();
        private readonly IProperty<double> _costNext = H.Property<double>(c => c
            .On(e => e.UnitPrice)
            .On(e => e.QtyAbsNext)
            .Set(e => e.QtyAbsNext * e.UnitPrice)
        );

        //[TriggerOn(nameof(SumRightQtyAbs)), NotMapped]
        [Ignore]
        public double QtyAbs => _qtyAbs.Get();
        private readonly IProperty<double> _qtyAbs = H.Property<double>(c => c
            //.On(e => e.SumRightQtyAbs)
            //.Set(e => e.SumRightQtyAbs)
        );


        [NotMapped]
        public double QtyAbsNext => _qtyAbsNext.Get();
        private readonly IProperty<double> _qtyAbsNext = H.Property<double>(c => c
            //.On(e => e.SumRightQtyAbsNext)
            //.Set(e => e.SumRightQtyAbsNext)
        );

    }

}
