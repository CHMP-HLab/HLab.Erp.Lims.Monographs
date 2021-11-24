using System.Linq;
using HLab.Base.Extentions;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using HLab.Notify.PropertyChanged;
using NPoco;

namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Monograph>;

    public partial class Monograph : IListableModel
    {
        [Ignore]
        public double ConsumablesCost => _consumablesCost.Get();

        private readonly IProperty<double> _consumablesCost = H.Property<double>(c => c
            .On(e => e.Consumables.Item().Cost)
            .Set(e => e.Consumables.Where(i => i.Cost.IsRegular()).Sum(i => i.Cost))
        );


        [Ignore]
        public double ConsumablesCostNext => _consumablesCostNext.Get();
        private readonly IProperty<double> _consumablesCostNext = H.Property<double>(c => c
            .On(e => e.Consumables.Item().CostNext)
            .Set(e => e.Consumables.Where(i => i.CostNext.IsRegular()).Sum(i => i.CostNext))
        );

        [Ignore]
        public double LabCost => _labCost.Get();
        private readonly IProperty<double> _labCost = H.Property<double>(c => c
            .On(e => e.Tests.Item().Cost)
            .Set(e => e.Tests.Where(i => i.Cost.IsRegular()).Sum(i => i.Cost))
        );

        [Ignore]
        public double LabCostNext => _labCostNext.Get();
        private readonly IProperty<double> _labCostNext = H.Property<double>(c => c
            .On(e => e.Tests.Item().CostNext)
            .Set(e => e.Tests.Where(i => i.CostNext.IsRegular()).Sum(i => i.CostNext))
        );

        [Ignore]
       public double Cost => _cost.Get();
        private readonly IProperty<double> _cost = H.Property<double>(c => c
            .On(e => e.ConsumablesCost)
            .On(e => e.LabCost)
            .Set(e => e.ConsumablesCost + e.LabCost)
        );

        [Ignore]
        public double CostNext => _costNext.Get();
        private readonly IProperty<double> _costNext = H.Property<double>(c => c
            .On(e => e.ConsumablesCostNext)
            .On(e => e.LabCostNext)
            .Set(e => e.ConsumablesCostNext + e.LabCostNext)
        );

        [Ignore]
        public string Caption => _caption.Get();

        private readonly IProperty<string> _caption = H.Property<string>(c => c
            .On(e => e.Inn.Name)
            .On(e => e.Form.Name)
            .On(e => e.Pharmacopoeia.Name)
            .On(e => e.PharmacopoeiaVersion)
            .Set(e =>
            {
                if(string.IsNullOrEmpty(e.Inn.Name)) return "{New monograph}";
                return "Monograph\n" + e.Inn?.Name + " " +
                                      e.Form?.Name + " " +
                                      e.Pharmacopoeia?.Name + " " +
                                      e.PharmacopoeiaVersion;
            })
        );

    }
}
