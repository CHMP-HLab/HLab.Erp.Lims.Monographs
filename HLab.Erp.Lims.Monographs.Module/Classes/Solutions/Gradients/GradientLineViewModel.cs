using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Gradients
{
    using H = H<GradientLineViewModel>;

    public class GradientLineViewModel : ViewModel<GradientLine>//, IObservableChartPoint
    {

        public GradientLineViewModel() => H.Initialize(this);

        [TriggerOn(nameof(Model), "Time")]
        public double Time
        {
            get => Model.Time ?? 0; set => Model.Time = value;
        }

        [TriggerOn(nameof(Model), "Ratio")]
        public double RatioPCent
        {
            get => 100 * Model?.Ratio ?? 0;
            set => Model.Ratio = value / 100;
        }

        public GradientViewModel Parent
        {
            get => _parent.Get();
            set => _parent.Set(value);
        }

        readonly IProperty<GradientViewModel> _parent = H.Property<GradientViewModel>();


        //[TriggerOn(nameof(Model)+".Time")]
        //[TriggerOn(nameof(Model)+".Ratio")]
        //private void OnPointChanged()
        //{
        //    PointChanged?.Invoke();
        //}

        //public event Action PointChanged;
    }
}
