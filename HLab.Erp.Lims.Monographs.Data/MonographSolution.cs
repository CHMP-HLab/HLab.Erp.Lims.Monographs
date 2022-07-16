using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<MonographSolution>;

    [Table("MonographieSolution")]
    public partial class MonographSolution : MonographLinkedElement
    {
        public MonographSolution() => H.Initialize(this);

        [Column("Quantite")]
        public double QtyMin
        {
            get => _qtyMin.Get();
            set => _qtyMin.Set(value);
        }

        private readonly IProperty<double> _qtyMin = H.Property<double>(c => c.Default(0.0));


        //[Column]
        //public double QuantiteSuivant
        //{
        //    get => N.Get(() => 0.0); set => N.Set(value);
        //}

        [Column("UniteId")]
        public int? UnitMinId
        {
            get => _unitMinId.Get();
            set => _unitMinId.Set(value);
        }

        private readonly IProperty<int?> _unitMinId = H.Property<int?>();

        [NotMapped]
        public Unit UnitMin
        {
            get => _unitMin.Get();
            set => UnitMinId = value.Id;
        }

        private readonly IProperty<Unit> _unitMin = H.Property<Unit>(c => c.Foreign(e => e.UnitMinId));


        [Column]
        public string Designation
        {
            get => _designation.Get();
            set => _designation.Set(value);
        }

        private readonly IProperty<string> _designation = H.Property<string>(c => c.Default(""));



        [Column("Commentaire")]
        public string Note
        {
            get => _note.Get();
            set => _note.Set(value);
        }

        private readonly IProperty<string> _note = H.Property<string>(c => c.Default(""));


        [Column("QuantiteMode")]
        public string QtyMode
        {
            get => _qtyMode.Get();
            set => _qtyMode.Set(value);
        }

        private readonly IProperty<string> _qtyMode = H.Property<string>(c => c.Default(""));


        [Column]
        public int? Order
        {
            get => _order.Get();
            set => _order.Set(value);
        }

        private readonly IProperty<int?> _order = H.Property<int?>();


        [Column]
        public double Top
        {
            get => _top.Get();
            set => _top.Set(value);
        }

        private readonly IProperty<double> _top = H.Property<double>(c => c.Default(0.0));


        [Column]
        public double Right
        {
            get => _right.Get();
            set => _right.Set(value);
        }

        private readonly IProperty<double> _right = H.Property<double>(c => c.Default(0.0));


        public override double CostNext { get; }

        [TriggerOn(nameof(QtyMode)), NotMapped]
        public override string UnitGroup
        {
            get
            {
                switch (QtyMode)
                {
                    case "f":
                        return "v";
                    case "m":
                        return "m";
                    case "u":
                        return "u";
                    case "g":
                        return "v";
                    default:
                        return "v";
                }
            }
        }

        [TriggerOn(nameof(QtyMode)), NotMapped]
        public override string RightUnitGroup
        {
            get
            {
                switch (QtyMode)
                {
                    case "f":
                        return "v";
                    case "m":
                        return "m";
                    case "u":
                        return "u";
                    case "g":
                        return "u";
                    default:
                        return "v";
                }
            }
        }

        public override double QtyAbs { get; }
        public override double QtyAbsNext { get; }
        public override double Cost { get; }
    }
}
