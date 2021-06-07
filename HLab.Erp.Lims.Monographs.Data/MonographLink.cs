using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Erp.Units;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<MonographLink>;
    public partial class MonographLink : Entity
    {
        public MonographLink() => H.Initialize(this);

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


        [Column("MonographieConsommableId")]
        public int? MonographConsumableId
        {
            get => _monographConsumableId.Get();
            set => _monographConsumableId.Set(value);
        }

        private readonly IProperty<int?> _monographConsumableId = H.Property<int?>();

        [NotMapped]
        public MonographConsumable MonographConsumable
        {
            get => _monographConsumable.Get();
            set => MonographConsumableId = value.Id;
        }

        private readonly IProperty<MonographConsumable> _monographConsumable = H.Property<MonographConsumable>(c => c.Foreign(e => e.MonographConsumableId));


        [Column("MonographieSolutionId")]
        public int? MonographSolutionId
        {
            get => _monographSolutionId.Get();
            set => _monographSolutionId.Set(value);
        }

        private readonly IProperty<int?> _monographSolutionId = H.Property<int?>();

        [NotMapped]
        public MonographSolution MonographSolution
        {
            get => _monographSolution.Get();
            set => MonographSolutionId = value.Id;
        }

        private readonly IProperty<MonographSolution> _monographSolution = H.Property<MonographSolution>(c => c.Foreign(e => e.MonographSolutionId));


        [Column("MonographieSolutionParentId")]
        public int? MonographSolutionParentId
        {
            get => _monographSolutionParentId.Get();
            set => _monographSolutionParentId.Set(value);
        }

        private readonly IProperty<int?> _monographSolutionParentId = H.Property<int?>();

        [NotMapped]
        public MonographSolution MonographSolutionParent
        {
            get => _monographSolutionParent.Get();
            set => MonographSolutionParentId = value.Id;
        }

        private readonly IProperty<MonographSolution> _monographSolutionParent = H.Property<MonographSolution>(c => c.Foreign(e => e.MonographSolutionParentId));


        [Column("MonographieTestId")]
        public int? MonographAssayId
        {
            get => _monographAssayId.Get();
            set => _monographAssayId.Set(value);
        }

        private readonly IProperty<int?> _monographAssayId = H.Property<int?>();

        [NotMapped]
        public MonographTest MonographAssay
        {
            get => _monographAssay.Get();
            set => MonographAssayId = value.Id;
        }

        private readonly IProperty<MonographTest> _monographAssay = H.Property<MonographTest>(c => c.Foreign(e => e.MonographAssayId));


        [Column("Quantite")]
        public double Qty
        {
            get => _qty.Get();
            set => _qty.Set(value);
        }

        private readonly IProperty<double> _qty = H.Property<double>();


        [Column("QuantiteSuivant")]
        public double QtyNext
        {
            get => _qtyNext.Get();
            set => _qtyNext.Set(value);
        }

        private readonly IProperty<double> _qtyNext = H.Property<double>();


        [Column("QuantiteRatio")]
        public double QtyRatio
        {
            get => _qtyRatio.Get();
            set => _qtyRatio.Set(value);
        }

        private readonly IProperty<double> _qtyRatio = H.Property<double>(c => c.Default(0.0));


        [Column("UniteId")]
        public int? UnitId
        {
            get => _unitId.Get();
            set => _unitId.Set(value);
        }

        private readonly IProperty<int?> _unitId = H.Property<int?>();

        [NotMapped]
        public Unit Unit
        {
            get => _unit.Get();
            set => UnitId = value.Id;
        }

        private readonly IProperty<Unit> _unit = H.Property<Unit>(c => c.Foreign(e => e.UnitId));


        [Column("UniteRatioId")]
        public int? UnitRatioId
        {
            get => _unitRatioId.Get();
            set => _unitRatioId.Set(value);
        }

        private readonly IProperty<int?> _unitRatioId = H.Property<int?>();

        [NotMapped]
        public Unit UnitRatio
        {
            get => _unitRatio.Get();
            set => UnitRatioId = value.Id;
        }

        private readonly IProperty<Unit> _unitRatio = H.Property<Unit>(c => c.Foreign(e => e.UnitRatioId));


        [Column("Commentaire")]
        public string Comment
        {
            get => _comment.Get();
            set => _comment.Set(value);
        }

        private readonly IProperty<string> _comment = H.Property<string>(c => c.Default(""));

    }
}
