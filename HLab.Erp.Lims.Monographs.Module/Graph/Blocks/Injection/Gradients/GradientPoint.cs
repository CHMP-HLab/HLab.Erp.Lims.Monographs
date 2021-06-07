using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients
{
    public class GradientPoint : NotifierBase
    {
        public GradientPoint(GradientChannel gradientChannel, GradientTime gradientTime)
        {
            GradientChannel = gradientChannel;
            GradientTime = gradientTime;
        }

        public GradientChannel GradientChannel { get; }
        public GradientTime GradientTime { get; }

        public double Ratio
        {
            get => _ratio.Get();
            set => _ratio.Set(value);
        }

        private readonly IProperty<double> _ratio = H<GradientPoint>.Property<double>(c => c.Default(0.0));
    }
}