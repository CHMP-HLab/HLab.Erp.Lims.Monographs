
using HLab.ColorTools.Wpf;
using HLab.Erp.Core.ViewModelStates;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Icons.Annotations.Icons;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Detail
{
    using H = H<TestClassViewModel>;

    public class TestClassViewModel : ViewModel<TestClass>
    {
        public IIconService IconService { get; private set; }
        public State State { get; private set; }

        public void Inject(IIconService iconService, State state)
        {
            IconService = iconService;
            State = state;
            H.Initialize(this);
        }


        public string IconName => _iconName.Get();
        private readonly IProperty<string> _iconName = H.Property<string>(c => c
            .On(e => e.Model.IconPath)
            .Set(e=>"Icons/" + e.Model?.IconPath ?? "IconMicroscope")
        );



        [TriggerOn(nameof(Model), "Color")]
        public void UpdateColor()
        {
            State.Color = (Model?.Color).ToColor();
        }

        [TriggerOn(nameof(State), "Color")]
        void OnStateColor()
        {
            if (Model == null) return;

            //TODO 
            //Model.Color = State.Color.ToInt();
        }

    }
}
