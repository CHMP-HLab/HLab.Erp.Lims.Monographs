using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using NPoco;


namespace HLab.Erp.Lims.Monographs.Data
{
    using H = HD<Gradient>;

    public partial class Gradient
    {
        [TriggerOn(nameof(PerInjectionVolumeMl)), Ignore]
        public double PerInjectionVolume => PerInjectionVolumeMl / 1000;

        [Ignore]
        public double PerInjectionVolumeMl => _perInjectionVolumeMl.Get();

        private readonly IProperty<double> _perInjectionVolumeMl = H.Property<double>(c => c
            .On(e => e.Lines.Item().Ratio)
            .On(e => e.Lines.Item().Time)
            .On(e => e.FlowRate)
            //.Set(e =>
            //{
            //    double volume = 0;
            //    GradientLine old = null;

            //    //lock (Lines.Lock)
            //    var lines = e.Lines.OrderBy(i => i.Time).ToList();

            //    foreach (var line in lines)
            //    {
            //        if (old != null)
            //        {
            //            double timespan = (line.Time ?? 0) - (old.Time ?? 0);
            //            volume += timespan * ((old.Ratio ?? 0) + (line.Ratio ?? 0));
            //        }
            //        old = line;
            //    }

            //    volume *= (e.FlowRate ?? 0) / 2;

            //    return volume;
            //})
                
        );



        [Ignore]
        public ObservableQuery<GradientLine> Lines
        {
            get => _lines.Get();
            set => _lines.Set(value);
        }

        private readonly IProperty<ObservableQuery<GradientLine>> _lines = H.Property<ObservableQuery<GradientLine>>(c => c
            //.Set(e => new ObservableQuery<GradientLine>(e.Context.Db) { }
            //    .AddFilter(gl => gl.GradientId == e.Id)
            //    .AddFilterFunc(q => q.OrderBy(gl => gl.Time), 1)
            //    .AddOnCreate(h =>
            //    {
            //        h.Entity.GradientId = e.Id;

            //        var before = h.List.Selected ?? h.List.Last();
            //        var after = h.List.FirstOrDefault(q => q.Time > before.Time);
            //        if (after != null)
            //        {
            //            h.Entity.Ratio = (before.Ratio + after.Ratio) / 2;

            //            h.Entity.Time = (before.Time + after.Time) / 2;
            //        }
            //        else
            //        {
            //            h.Entity.Ratio = before.Ratio;
            //            h.Entity.Time = before.Time + 5;
            //        }

            //        h.Done = true;
            //    })
            //    .AddOnDelete(h => h.Done = true)
            //    .FluentUpdate())
        );

    }
}
