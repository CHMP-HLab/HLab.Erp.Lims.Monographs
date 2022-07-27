using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<MonographTest>;
    public partial class MonographTest
    {
        [NotMapped]
        public double HourlyCost => _hourlyCost.Get();

        readonly IProperty<double> _hourlyCost = H.Property<double>(c => c.Default(100.0));

        [NotMapped]
        [TriggerOn(nameof(TestClass), "DurationFirst")]
        [TriggerOn(nameof(HourlyCost))]
        //[TriggerOn(nameof(SumLeftCost))]
        public double Cost => (TestClass?.DurationFirst ?? 0) * (HourlyCost/60);

        [NotMapped]
        [TriggerOn(nameof(TestClass), "DurationNext")]
        [TriggerOn(nameof(HourlyCost))]
        //[TriggerOn(nameof(SumLeftCostNext))]
        public double CostNext => (TestClass?.DurationNext ?? 0) * (HourlyCost/60);

        [Ignore]
        public double QtyAbs => 1;
        [Ignore]
        public double QtyAbsNext => 1;

    }
}
