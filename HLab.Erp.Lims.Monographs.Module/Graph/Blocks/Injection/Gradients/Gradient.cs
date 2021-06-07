using System.Collections.ObjectModel;
using HLab.Base.Extensions;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients
{
    public class Gradient : NotifierBase
    {
        public GradientChannel GetOrAddChannel(string name) 
            => Channels.GetOrAdd(c => c.Name == name, () => new GradientChannel(this) {Name = name});
        public GradientTime GetOrAddTime(double time)
            => Times.GetOrAdd(t => t.Time == time, () => new GradientTime(this) { Time = time });
        public GradientPoint GetOrAddPoint(GradientChannel channel, GradientTime time)
            => Points.GetOrAdd(
                p => p.GradientChannel==channel && p.GradientTime == time, 
                () => new GradientPoint(channel,time));
        public ObservableCollection<GradientChannel> Channels { get; } = new ObservableCollection<GradientChannel>();
        public ObservableCollection<GradientTime> Times { get; } = new ObservableCollection<GradientTime>();
        public ObservableCollection<GradientPoint> Points { get; } = new ObservableCollection<GradientPoint>();

        public void Set(string channel, double time, double ratio)
        {
            var c = GetOrAddChannel(channel);
            var t = GetOrAddTime(time);
            var p = GetOrAddPoint(c, t);
            p.Ratio = ratio;
        }
    }
}