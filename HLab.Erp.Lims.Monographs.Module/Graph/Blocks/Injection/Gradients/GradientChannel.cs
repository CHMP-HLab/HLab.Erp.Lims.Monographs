using System.Linq;
using HLab.Notify;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients
{
    using H = H<GradientChannel>;
    public class GradientChannel : NotifierBase
    {
        public GradientChannel(Gradient gradient)
        {
            Gradient = gradient;
        }

        public Gradient Gradient { get; }
        public string Name
        {
            get => _name.Get();
            set => _name.Set(value);
        }

        private readonly IProperty<string> _name = H.Property<string>();

        public void AdjustPoints()
        {
            var points = Points.ToList();

            foreach (var time in Gradient.Times)
            {
                var point = points.FirstOrDefault(p => ReferenceEquals(p.GradientTime, time));
                if (point == null)
                {
                    Gradient.Points.Add(new GradientPoint(this, time));
                }
                else points.Remove(point);
            }

            foreach (var point in points)
            {
                Gradient.Points.Remove(point);
            }
        }

        public IObservableFilter<GradientPoint> Points { get; } = H.Filter<GradientPoint>(c => c
                .AddFilter((e,i) => ReferenceEquals(i.GradientChannel, e))
                .Link(e => e.Gradient.Points)            
            );

        public double Ratio => _ratio.Get();

        private readonly IProperty<double> _ratio = H.Property<double>(c => c
            .On(e => e.Points.Item().GradientTime.Time)
            .On(e => e.Points.Item().Ratio)
            .Set(e =>
                {
                    var ratio = 0.0;
                    var points = e.Points.OrderBy(p => p.GradientTime.Time).ToList();
                    GradientPoint old = null;
                    foreach (var point in points)
                    {
                        if (old != null)
                        {
                            var timespan = point.GradientTime.Time - old.GradientTime.Time;
                            ratio += timespan * (old.Ratio + point.Ratio);
                        }
                        old = point;
                    }

                    ratio /= 2 * old?.GradientTime.Time??1.0;

                    return ratio;
                    
                }
            )
        );
    }
}