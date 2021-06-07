//using System.ComponentModel.DataAnnotations.Schema;
//using HLab.DependencyInjection.Annotations;
//using HLab.Erp.Data;
//using HLab.Erp.Data.Observables;
////using HLab.Erp.Units;
//using HLab.Notify.Annotations;
//using HLab.Notify.PropertyChanged;

//namespace HLab.Erp.Lims.Monographs.Data
//{
//    public partial class MonographLink
//    {
//        [Import]
//        private readonly IDataService _db;

//        public double Cost => _cost.Get();

//        private readonly IProperty<double> _cost = H.Property<double>(c => c
//            .On(e => e.QtyAbs)
//            .On(e => e.Left.Cost)
//            .On(e => e.Left.SumRightQtyAbs)
//            .Set(e => e.QtyAbs * (e.Left?.Cost ?? 0) / (e.Left?.SumRightQtyAbs ?? 1)));


//        public double CostNext => _costNext.Get();

//        private readonly IProperty<double> _costNext = H.Property<double>(c => c
//            .On(e => e.QtyAbsNext)
//            .On(e => e.Left.CostNext)
//            .On(e => e.Left.SumRightQtyAbsNext)
//            .Set(e => e.QtyAbsNext * (e.Left?.CostNext ?? 0) / (e.Left?.SumRightQtyAbsNext ?? 1)));


//        public IMonographLinkedElement Left => _left.Get();

//        private readonly IProperty<IMonographLinkedElement> _left = H.Property<IMonographLinkedElement>(c => c
//            .On(e => e.MonographSolution)
//            .On(e => e.MonographConsumable)
//            .Set(e => e.MonographConsumableId != null
//                ? (IMonographLinkedElement) e.MonographConsumable
//                : e.MonographSolution)
//        );


//        public IMonographLinkedElement Right => _right.Get();

//        private readonly IProperty<IMonographLinkedElement> _right = H.Property<IMonographLinkedElement>(c => c
//            .On(e => e.MonographAssay)
//            .On(e => e.MonographSolutionParent)
//            .Set(e => e.MonographAssayId == null
//                ? (IMonographLinkedElement) e.MonographSolutionParent
//                : e.MonographAssay));


//        [TriggerOn(nameof(Left), "RightUnitGroup")]
//        public string UnitGroup
//            => Left?.RightUnitGroup;


//        public Consumable Consumable => _consumable.Get();

//        private readonly IProperty<Consumable> _consumable = H.Property<Consumable>(c => c
//            .On(e => e.MonographConsumable.Consumable)
//            .Set(e => e.MonographConsumable?.Consumable)
//        );


//        public TarifFournisseur TarifFournisseur
//        {
//            get => _tarifFournisseur.Get();
//            set => _tarifFournisseur.Set(value);
//        }

//        private readonly IProperty<TarifFournisseur> _tarifFournisseur = H.Property<TarifFournisseur>(c => c
//            .On(e => e.MonographConsumable.TarifFournisseur)
//            .On(e => e.Consumable.TarifFournisseurDefaultId)
//            .Set(e =>
//            {
//                var tf = e.MonographConsumable?.TarifFournisseur;
//                if (tf != null) return tf;

//                var id = e.Consumable?.TarifFournisseurDefaultId;
//                return id != null ? e._db.FetchOne<TarifFournisseur>(id.Value) : null;

//            })
//        );



//        public double? MolarMass => _molarMass.Get(); 

//        private readonly IProperty<double?> _molarMass = H.Property<double?>(c => c
//            .On(e => e.MonographConsumable.Consumable.MolarMass)
//            .Set(e => e.MonographConsumable.Consumable.MolarMass)
//        );


//        [NotMapped]
//        public ObservableQuery<Unit> UniteRatioList => _uniteRatioList.Get();
//        private readonly IProperty<ObservableQuery<Unit>> _uniteRatioList = H.Property<ObservableQuery<Unit>>(c => c
//            .On(e => e.UniteRatioGroup)
//                .Set(e => new ObservableQuery<Unit>(e.Context.Db) { }
//                    .AddFilter(f => f.Group == e.UniteRatioGroup).FluentUpdate())
//        );

//        [TriggerOn(nameof(Right),"UnitGroup")]
//       public string UniteRatioGroup => Right?.UnitGroup;

//        [NotMapped]
//        public ObservableQuery<Unit> Units => _units.Get();
//        private readonly IProperty<ObservableQuery<Unit>> _units = H.Property<ObservableQuery<Unit>>(c => c
//            .On(e => e.UnitGroup)
//            .Set(e => new ObservableQuery<Unit>(e.Context.Db) { }
//                .AddFilter(f => f.Group == e.UnitGroup).FluentUpdate())
//        );

//        [NotMapped]
//        public Unit LeftUnit => _leftUnit.Get();
//        private readonly IProperty<Unit> _leftUnit = H.Property<Unit>(c => c

//            .On(e => e.UnitGroup)
//            .On(e => e.Units.Item())
//            .Set(e => e.Units.BestMatch(e.QtyAbs))
//            );


//        //Used to show resulting quantity
//        [NotMapped]
//        public double LeftQty => _leftQty.Get();
//        private readonly IProperty<double> _leftQty = H.Property<double>(c => c
//            .On(e => e.QtyAbs)
//            .On(e => e.LeftUnit.Abs)
//            .Set(e => e.LeftUnit?.Qty(e.QtyAbs) ?? e.QtyAbs));


//        [NotMapped]
//        public double LeftQtyNext => _leftQtyNext.Get();
//        private readonly IProperty<double> _leftQtyNext = H.Property<double>(c => c
//            .On(e => e.QtyAbsNext)
//            .On(e => e.LeftUnit.Abs)
//            .Set(e => e.LeftUnit?.Qty(e.QtyAbsNext) ?? e.QtyAbsNext));

//        [NotMapped]
//        public double QsQty => _qsQty.Get();
//        private readonly IProperty<double> _qsQty = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.Right.QtyAbs)
//            .On(e => e.Right.MainQtyAbs)
//            .Set(e =>
//            {
//                if (e.LeftUnit?.Group != "qs") return 0;
//                return e.Right?.QtyAbs??0 - e.Right?.MainQtyAbs??0;
                
//            }
//            ));


//        [NotMapped]
//        public double QsQtyNext => _qsQtyNext.Get();
//        private readonly IProperty<double> _qsQtyNext = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.Right.QtyAbsNext)
//            .On(e => e.Right.MainQtyAbsNext)
//            .Set(e =>
//                {
//                    if (e.LeftUnit?.Group != "qs") return 0;
//                    return e.Right?.QtyAbsNext ?? 0 - e.Right?.MainQtyAbsNext ?? 0;
//                }
//            ));

//        private double GetMolarMass()
//        {
//            if (MolarMass == null) return 0;
//                var molar = MolarMass.Value;

//                if (Left?.UnitGroup == "v")
//                {
//                    //Prise en compte de la concentration
//                    var assay = TarifFournisseur?.Assay ?? 1;
//                    molar /= assay;

//                    //grammes => litres
//                    var density = TarifFournisseur?.Density ?? 1;
//                    molar /= density * 1000;
//                }
//            return molar;
//        }

//        [NotMapped]
//        public double MolarQty => _molarQty.Get();
//        private readonly IProperty<double> _molarQty = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.MolarMass)
//            .On(e => e.Left.UnitGroup)
//            .On(e => e.TarifFournisseur.Assay)
//            .On(e => e.TarifFournisseur.Density)
//            .On(e => e.Qty)
//            .Set(e => (e.LeftUnit?.Group == "mol")?e.Qty * e.GetMolarMass():0)
//        );


//        [NotMapped]
//        public double MolarQtyNext => _molarQtyNext.Get();
//        private readonly IProperty<double> _molarQtyNext = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.MolarMass)
//            .On(e => e.Left.UnitGroup)
//            .On(e => e.TarifFournisseur.Assay)
//            .On(e => e.TarifFournisseur.Density)
//            .On(e => e.QtyNext)
//            .Set(e => (e.LeftUnit?.Group == "mol") ? e.QtyNext * e.GetMolarMass() : 0)
//        );



//        [NotMapped]
//        public double RegularQty => _regularQty.Get();
//        private readonly IProperty<double> _regularQty = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Abs)
//            .On(e => e.Qty)
//            .Set(e =>
//            {
//                switch (e.LeftUnit?.Group)
//                {
//                    case "qs":
//                    case "pc":
//                    case "vol":
//                        return 0;
//                    default:
//                        return e.LeftUnit?.AbsQty(e.Qty)??0;
//                }
//            })
//        );


//        [NotMapped]
//        public double RegularQtyNext => _regularQtyNext.Get();
//        private readonly IProperty<double> _regularQtyNext = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Abs)
//            .On(e => e.QtyNext)
//            .Set(e => e.LeftUnit?.AbsQty(e.QtyNext)??0)
//        );


//        [NotMapped]
//        public double PercentQty => _percentQty.Get();
//        private readonly IProperty<double> _percentQty = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.Right.QtyAbs)
//            .On(e => e.Qty)
//            .Set(e => (e.LeftUnit?.Group == "pc") ? (e.Right?.QtyAbs ?? 0) * e.Qty / 100 : 0)
//        );

//        [NotMapped]
//        public double PercentQtyNext => _percentQtyNext.Get();
//        private readonly IProperty<double> _percentQtyNext = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.Right.QtyAbsNext)
//            .On(e => e.QtyNext)
//            .Set(e => (e.LeftUnit?.Group == "pc")?(e.Right?.QtyAbsNext??0) * e.QtyNext / 100:0)
//        );


//        [NotMapped]
//        public double VolQty => _volQty.Get();
//        private readonly IProperty<double> _volQty = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.Right.QtyAbs)
//            .On(e => e.Right.NbVolumes)
//            .On(e => e.Qty)
//            .Set(e => (e.LeftUnit?.Group == "vol") ? (e.Right?.QtyAbs ?? 0) * e.Qty / (e.Right?.NbVolumes ?? 1) : 0)
//        );

//        [NotMapped]
//        public double VolQtyNext => _volQtyNext.Get();
//        private readonly IProperty<double> _volQtyNext = H.Property<double>(c => c
//            .On(e => e.LeftUnit.Group)
//            .On(e => e.Right.QtyAbsNext)
//            .On(e => e.Right.NbVolumesNext)
//            .On(e => e.QtyNext)
//            .Set(e => (e.LeftUnit?.Group == "vol")?(e.Right?.QtyAbsNext??0) * e.QtyNext / (e.Right?.NbVolumesNext??1):0)
//        );


//        [NotMapped]
//        public double Ratio => _ratio.Get();
//        private readonly IProperty<double> _ratio = H.Property<double>(c => c
//            .On(e => e.UnitRatio.Abs)
//            .On(e => e.QtyRatio)
//            .On(e => e.Right.QtyAbs)
//            .Set(e =>
//            {
//                switch (e.LeftUnit?.Group)
//                {
//                    case "qs":
//                    case "pc":
//                    case "vol":
//                        return 1;
//                    default:
//                        if (e.UnitRatio == null) return 1;
//                        return (e.Right?.QtyAbs ?? 0) / e.UnitRatio.AbsQty(e.QtyRatio);
//                }
//            })
//        );

//        [NotMapped]
//        public double RatioNext => _ratioNext.Get();
//        private readonly IProperty<double> _ratioNext = H.Property<double>(c => c
//            .On(e => e.UnitRatio.Abs)
//            .On(e => e.QtyRatio)
//            .On(e => e.Right.QtyAbsNext)
//            .Set(e =>
//            {
//                    switch (e.LeftUnit?.Group)
//                    {
//                        case "qs":
//                        case "pc":
//                        case "vol":
//                            return 1;
//                        default:
//                            if (e.UnitRatio == null) return 1;
//                            return  (e.Right?.QtyAbsNext??0) / e.UnitRatio.AbsQty(e.QtyRatio);
//                    }
//            })
//        );


//        [NotMapped]
//        public double QtyAbs => _qtyAbs.Get();
//        private readonly IProperty<double> _qtyAbs = H.Property<double>(c => c
//            .On(e => e.QsQtyNext)
//            .On(e => e.PercentQtyNext)
//            .On(e => e.VolQtyNext)
//            .On(e => e.RegularQtyNext)
//            .On(e => e.RatioNext)
//            .Set(e => (e.QsQty + e.PercentQty + e.VolQty + e.RegularQty) * e.Ratio)
//        );

//        [NotMapped]
//        public double QtyAbsNext => _qtyAbsNext.Get();
//        private readonly IProperty<double> _qtyAbsNext = H.Property<double>(c => c
//            .On(e => e.QsQtyNext)
//            .On(e => e.PercentQtyNext)
//            .On(e => e.VolQtyNext)
//            .On(e => e.RegularQtyNext)
//            .On(e => e.RatioNext)
//            .Set(e => (e.QsQtyNext + e.PercentQtyNext + e.VolQtyNext + e.RegularQtyNext) * e.RatioNext )
//            );


//        public void SetDefaultValue(string unitGroup)
//        {
//            var u = _db.FetchOne<Unit>(e => e.Group == unitGroup && e.Default);

//            if (u != null)
//            {
//                //UnitId = u;
//                Qty = u.DefaultQty;
//                QtyNext = u.DefaultQty;
//            }

//            SetDefaultRatio();
//        }


//        public void SetDefaultRatio()
//        {
//            if (Right == null) return;
//            if (UnitRatio?.Group == Right.UnitGroup) return;
//            var unit = _db.FetchOne<Unit>(e => e.Group == Right.UnitGroup && e.Default); ;
//            UnitRatioId = unit?.Id;
//            QtyRatio = 1;//unit?.DefaultQty ?? 0;
//        }

//        //Todo : [Hack] check why this is needed, cause I think it should not
//        public void UpdateLinks()
//        {
//            //Left.RightLinks.FluentUpdate();
//            //Right.LeftLinks.FluentUpdate();
//            Monograph.Links.FluentUpdate();
//        }
//    }
//}
