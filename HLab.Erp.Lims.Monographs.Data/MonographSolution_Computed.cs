//using System;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Dynamic;
//using HLab.Mvvm.Observables;
//using HLab.Notify.Annotations;
//using HLab.Notify.PropertyChanged;

//namespace HLab.Erp.Lims.Monographs.Data
//{
//    public partial class MonographSolution
//    {
//        public override IObservableFilter<MonographLink> LeftLinks { get; } =
//            H.Filter<MonographLink>((e,f) => f.AddFilter(i => i.MonographSolutionParentId == e.Id));

//        public override IObservableFilter<MonographLink> RightLinks { get; } =
//        H.Filter<MonographLink>((e, f) => f.AddFilter(i => i.MonographSolutionId == e.Id));
        
//        [NotMapped]
//        public double GradientQty => _gradientQty.Get();

//        private readonly IProperty<double> _gradientQty = H.Property<double>(c => c
//            .On(e => e.QtyMode)
//            .On(e => e.Gradient.PerInjectionVolume)
//            .On(e => e.SumRightQtyAbs)
//            .Set(e =>
//            {
//                if (e.QtyMode != "g") return 0;
//                // TODO : if (Gradient == null) this.GetNotifier().UnSet(this.GetType().GetProperty("Gradient"));
//                if (e.Gradient == null) return 0;

//                return (1.1 * e.SumRightQtyAbs * e.Gradient.PerInjectionVolume) + 0.150;
//            })

//        );

//        [NotMapped]
//        public double GradientQtyNext => _gradientQtyNext.Get();
//        private readonly IProperty<double> _gradientQtyNext = H.Property<double>(c => c
//            .On(e => e.QtyMode)
//            .On(e => e.Gradient.PerInjectionVolume)
//            .On(e => e.SumRightQtyAbsNext)
//            .Set(e =>
//            {
//                if (e.QtyMode != "g") return 0;
//                // TODO : if (Gradient == null) this.GetNotifier().UnSet(this.GetType().GetProperty("Gradient"));
//                if (e.Gradient == null) return 0;

//                return (1.1 * e.SumRightQtyAbsNext * e.Gradient.PerInjectionVolume);
//            })

//        );


//        [NotMapped]
//        [TriggerOn(nameof(SumRightQtyAbs))]
//        [TriggerOn(nameof(QtyMinAbs))]
//        public double RegularQty => _regularQty.Get();

//        private readonly IProperty<double> _regularQty = H.Property<double>(c => c
//            .On(e => e.SumRightQtyAbs)
//            .On(e => e.QtyMinAbs)
//            .Set(e =>
//            {
//                if (e.QtyMode == "g") return 0;
//                return Math.Max(e.SumRightQtyAbs, e.QtyMinAbs);
//            })
//        );

//        [NotMapped]
//        [TriggerOn(nameof(SumRightQtyAbsNext))]
//        [TriggerOn(nameof(QtyMinAbs))]
//        [TriggerOn(nameof(NbAnalysisMax))]
//        public double RegularQtyNext => _regularQtyNext.Get();
//        private readonly IProperty<double> _regularQtyNext = H.Property<double>(c => c
//            .On(e => e.SumRightQtyAbsNext)
//            .On(e => e.QtyMinAbs)
//            .On(e => e.NbAnalysisMax)
//            .Set(e =>
//            {
//                if (e.QtyMode == "g") return 0;
//                if (e.NbAnalysisMax < 2) return e.SumRightQtyAbsNext;
//                return 0;
//            })
//        );


//        [NotMapped]
//        public double QtyMinAbs => _qtyMinAbs.Get();

//        private readonly IProperty<double> _qtyMinAbs = H.Property<double>(c => c
//            .On(e => e.QtyMin)
//            .On(e => e.UnitMin.Abs)
//            .Set(e => e.UnitMin?.AbsQty(e.QtyMin) ?? 0));

//        [NotMapped]
//        public override double QtyAbs => _qtyAbs.Get();
//        private readonly IProperty<double> _qtyAbs = H.Property<double>(c => c
//            .On(e => e.GradientQty)
//            .On(e => e.RegularQty)
//            .Set(e => e.GradientQty + e.RegularQty)
//        );


//        [NotMapped]
//        public override double QtyAbsNext => _qtyAbsNext.Get();
//        private readonly IProperty<double> _qtyAbsNext = H.Property<double>(c => c
//            .On(e => e.GradientQtyNext)
//            .On(e => e.RegularQtyNext)
//            .Set(e => e.GradientQtyNext + e.RegularQtyNext)
//        );


//        [NotMapped]
//        public int NbAnalysisMax => _nbAnalysisMax.Get();
//        private readonly IProperty<int> _nbAnalysisMax = H.Property<int>(c => c
//            .On(e => e.QtyMinAbs)
//            .On(e => e.SumRightQtyAbs)
//            .On(e => e.SumRightQtyAbsNext)
//            .Set(e =>
//                {
//                    if (e.QtyMinAbs < e.SumRightQtyAbs) return 0;
//                    if (e.QtyMinAbs < e.SumRightQtyAbs + e.SumRightQtyAbsNext) return 1;
//                    return 1 + (int)Math.Floor((e.QtyMinAbs - e.SumRightQtyAbs) / e.SumRightQtyAbsNext);
//                })
//        );

//        [NotMapped]
//        public override double Cost => _cost.Get();
//        private readonly IProperty<double> _cost = H.Property<double>(c => c
//            .On(e => e.SumLeftCost)
//            .Set(e => e.SumLeftCost)
//        );


//        [NotMapped]
//        [TriggerOn(nameof(SumLeftCostNext))]
//        public override double CostNext => _costNext.Get();
//        private readonly IProperty<double> _costNext = H.Property<double>(c => c
//            .On(e => e.SumLeftCostNext)
//            .Set(e => e.SumLeftCostNext)
//        );
//    }

//}
