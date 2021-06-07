using System.Linq;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients
{
    public class GradientTime : NotifierBase
    {
        public GradientTime(Gradient gradient)
        {
            Gradient = gradient;
        }
        public Gradient Gradient { get; }
        public double Time
        {
            get => _time.Get();
            set => _time.Set(value);
        }

        private readonly IProperty<double> _time = H<GradientTime>.Property<double>(c => c.Default(0.0));

        public void AdjustPoints()
        {
            var points = Points.ToList();

            foreach (var channel in Gradient.Channels)
            {
                var point = points.FirstOrDefault(p => ReferenceEquals(p.GradientChannel, channel));
                if (point == null)
                {
                    Gradient.Points.Add(new GradientPoint(channel, this));
                }
                else points.Remove(point);
            }

            foreach (var point in points)
            {
                Gradient.Points.Remove(point);
            }
        }

        public IObservableFilter<GradientPoint> Points { get; } = H<GradientTime>.Filter<GradientPoint>(c => c
                .AddFilter((e,i) => ReferenceEquals(i.GradientTime, e))
                .Link(e => e.Gradient.Points)
        );
    }
}