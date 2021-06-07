using System.Collections.Generic;
using System.Linq;

using HLab.ColorTools.Wpf;
using HLab.Erp.Core.ViewModelStates;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Detail
{
    using H = H<ConsumableViewModel>;

    public class ConsumableViewModel : ViewModel<Consumable>
    {
        public ConsumableViewModel() => H.Initialize(this);

        public State State
        {
            get => _state.Get();
            set => _state.Set(value.SetColor(() => Model.Type.Color.ToColor()));
        }

        private readonly IProperty<State> _state = H.Property<State>(c => c
            .On(e => e.Model.Type.Color)
            .Do(e => e.State.OnTriggered())
            );


        public Dictionary<string,string> UniteGroupList => new Dictionary<string,string>
        {
            {"m","Masse"},
            {"v","Volume"},
            {"t","Temps"},
            {"u", "Unité"},
        };

        public ObservableQuery<ConsumableType> TypeList
        {
            get => _typeList.Get();
            set => _typeList.Set(value.FluentUpdate());
        }

        private readonly IProperty<ObservableQuery<ConsumableType>> _typeList = H.Property<ObservableQuery<ConsumableType>>(c => c
            //.On(e => e)
            //.Update()
        );




        [TriggerOn(nameof(Model) , "UnitGroup")]
        public KeyValuePair<string,string> UniteGroup
        {
            get => UniteGroupList.FirstOrDefault(e => e.Key == Model.UnitGroup);
            set => Model.UnitGroup = value.Key;
        }

    }
}

