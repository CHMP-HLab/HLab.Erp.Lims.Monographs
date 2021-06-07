using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = H<MonographLinkedElement>;
    public abstract class MonographLinkedElement : Entity, IMonographLinkedElement
    {
        protected MonographLinkedElement() => H.Initialize(this);

        //[Import, TriggerOn(nameof(Monograph), "Links"), Ignore]
        //public abstract IObservableFilter<MonographLink> LeftLinks { get; }

        //[Import, TriggerOn(nameof(Monograph), "Links"), Ignore]
        //public abstract IObservableFilter<MonographLink> RightLinks { get; }

        public bool LeftLinkedTo(IMonographLinkedElement e)
        {
            //if (LeftLinks == null) return false;
            //return ReferenceEquals(this, e) || LeftLinks.Any(l => l.Left.LeftLinkedTo(e));
            return false;
        }

        public bool RightLinkedTo(IMonographLinkedElement e)
        {
            //if (RightLinks == null) return false;
            //return ReferenceEquals(this, e) || RightLinks.Any(l => l.Right.RightLinkedTo(e));
            return false;
        }


        public abstract string UnitGroup { get; }
        public abstract string RightUnitGroup { get; }
        //public Unit Unit
        //{
        //    get => this.Get<Unit>(); protected set => N.Set(value);
        //}
        //public Unit RightUnit
        //{
        //    get => this.Get<Unit>(); protected set => N.Set(value);
        //}

        public abstract double QtyAbs { get; }
        public abstract double QtyAbsNext { get; }



        public double PerLeftQuantiteTarif(double qty = 1)
            => qty * Cost / QtyAbs;

        public abstract double Cost { get; }
        public abstract double CostNext { get; }

        public double PerLeftQtyCostNext(double qty = 1)
            => qty * CostNext / QtyAbsNext;

        public double PerRightQtyCost(double qty = 1)
            => qty * Cost / QtyAbs;

        public double PerRightQtyCostNext(double qty = 1)
            => qty * CostNext / QtyAbsNext;


        [Ignore]
        public double SumRightQtyAbs => _sumRightQtyAbs.Get();

        private readonly IProperty<double> _sumRightQtyAbs = H.Property<double>(c => c
            //.On(e => e.RightLinks.Item().QtyAbs)
            //.Set(e => e.RightLinks.Sum(link => link.QtyAbs))
            );

        [Ignore]
        public double SumRightQtyAbsNext => _sumRightQtyAbsNext.Get();

        private readonly IProperty<double> _sumRightQtyAbsNext = H.Property<double>(c => c
            //.On(e => e.RightLinks.Item().QtyAbsNext)
            //.Set(e => e.RightLinks.Sum(link => link.QtyAbsNext))
        );

        [Ignore]
        public double MainQtyAbs => _mainQtyAbs.Get();

        private readonly IProperty<double> _mainQtyAbs = H.Property<double>(c => c
            //.On(e => e.LeftLinks.Item().QtyAbs)
            //.On(e => e.LeftLinks.Item().LeftUnit.Group)
            //.Set(e => e.LeftLinks.Where(i => i.LeftUnit?.Group != "qs").Sum(i => i.QtyAbs))
        );


        [NotMapped]
        public double MainQtyAbsNext => _mainQtyAbsNext.Get();
        private readonly IProperty<double> _mainQtyAbsNext = H.Property<double>(c => c
            //.On(e => e.LeftLinks.Item().QtyAbsNext)
            //.On(e => e.LeftLinks.Item().LeftUnit.Group)
            //.Set(e => e.LeftLinks.Where(i => i.LeftUnit?.Group != "qs").Sum(i => i.QtyAbsNext))
        );

        [NotMapped]
        public double SumLeftCost => _sumLeftCost.Get();

        private readonly IProperty<double> _sumLeftCost = H.Property<double>(c => c
            //.On(e => e.LeftLinks.Item().Cost)
            //.Set(e => e.LeftLinks?.Where(l => l.Cost.IsRegular()).Sum(l => l.Cost) ?? 0)
        );


//        [TriggerOn(nameof(LeftLinks), "Item", "CostNext"), NotMapped]
        public double SumLeftCostNext => _sumLeftCostNext.Get();
        private readonly IProperty<double> _sumLeftCostNext = H.Property<double>(c => c
            //.On(e => e.LeftLinks.Item().Cost)
            //.Set(e => e.LeftLinks?.Where(l => l.CostNext.IsRegular()).Sum(l => l.CostNext) ?? 0)
        );


        [NotMapped]
        public double NbVolumes => _nbVolumes.Get();

        private readonly IProperty<double> _nbVolumes = H.Property<double>(c => c
            //.On(e => e.LeftLinks.Item().Qty)
            //.On(e => e.LeftLinks.Item().LeftUnit.Group)
            //.Set(e => e.LeftLinks.Where(i => i.LeftUnit != null && i.LeftUnit.Group == "vol").Sum(i => i.Qty))
        );

        [NotMapped]
        public double NbVolumesNext => _nbVolumesNext.Get();

        private readonly IProperty<double> _nbVolumesNext = H.Property<double>(c => c
            //.On(e => e.LeftLinks.Item().QtyNext)
            //.On(e => e.LeftLinks.Item().LeftUnit.Group)
            //.Set(e => e.LeftLinks.Where(i => i.LeftUnit != null && i.LeftUnit.Group == "vol").Sum(i => i.QtyNext))
        );

        [NPoco.Column("MonographieId")]
        public int? MonographId
        {
            get => _monographId.Get();
            set => _monographId.Set(value);
        }

        private readonly IProperty<int?> _monographId = H.Property<int?>();

        [NotMapped]
        public Monograph Monograph
        {
            get => _monograph.Get();
            set => MonographId = value.Id;
        }

        private readonly IProperty<Monograph> _monograph = H.Property<Monograph>(c => c.Foreign(e => e.MonographId));


    }
}