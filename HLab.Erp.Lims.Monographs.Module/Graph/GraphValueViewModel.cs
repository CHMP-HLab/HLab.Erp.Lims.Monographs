using System.Collections.Generic;
using System.Linq;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{
    using H = H<MonographValueViewModel>;

    internal class MonographValueViewModel : ViewModel<MonographValue>
    {
        readonly IDataService _db;

        public MonographValueViewModel(IDataService db)
        {
            _db = db;
            H.Initialize(this);
        }

        public List<Unit> Units => _units.Get();

        readonly IProperty<List<Unit>> _units = H.Property<List<Unit>>(c => c
            .Set(async e => await e._db.FetchAsync<Unit>().Where(u => u.UnitClass.Symbol == ((MonographValueType)e.Model.Type).UnitGroup).ToListAsync()));



        public double Value
        {
            get => _value.Get();
            set => _value.Set(Model.Value = Unit.AbsQty(value));
        }

        readonly IProperty<double> _value = H.Property<double>(c => c
            .On(e => e.Model.Value)
            .On(e => e.Unit)
            .Set(e => e.Unit.Qty(e.Model.Value)));


        public Unit Unit
        {
            get => _unit.Get();
            set => _unit.Set(value);
        }

        readonly IProperty<Unit> _unit = H.Property<Unit>();


        [TriggerOn]
        [TriggerOn(nameof(Model),"Value")]
        public void UpdateValue()
        {
            var v = Model.Value;
            Unit = Units.BestMatch(v);
        }
    }
}
