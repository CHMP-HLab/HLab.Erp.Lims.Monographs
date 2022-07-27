using System.Collections.Specialized;
using System.Linq;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients
{
    using H = H<GradientViewModel>;
    public class GradientViewModel : ViewModel<Gradient>
    {
        public GradientViewModel()
        {
            //var config = Mappers.Xy<GradientPoint>()
            //    .Y(g => g.Ratio)
            //    .X(g => g.GradientTime.Time);

            //Ratios = new SeriesCollection(config);

            H.Initialize(this);
        }

        [TriggerOn(nameof(Model))]
        public void OnModelChanged(object s, NotifierPropertyChangedEventArgs a)
        {
            if (a.OldValue is Gradient oldG)
            {
                oldG.Channels.CollectionChanged -= Channels_CollectionChanged;
            }
            if (a.NewValue is Gradient newG)
            {
                newG.Channels.CollectionChanged += Channels_CollectionChanged;
            }
        }

        void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems.OfType<GradientChannel>())
            {
                AddSerie(item);
            }
        }

        public void AddSerie(GradientChannel channel)
        {
            // TODO LiveCharts
            
            //var values = new ChartValues<GradientPoint>();
            //var series = new StackedAreaSeries(values);

            //Ratios.Add(series);

            //channel.Points.CollectionChanged += (s, a) =>
            //{
            //    if (a.Action == NotifyCollectionChangedAction.Add)
            //    {
            //        foreach (var n in a.NewItems.OfType<GradientPoint>())
            //        {
            //            int i = 0;
            //            if(values.Count>0)
            //            for (; values[i].GradientTime.Time < n.GradientTime.Time; i++)
            //            { }
            //            values.Insert(i,n);
            //        }
            //    }
            //    if (a.Action == NotifyCollectionChangedAction.Remove)
            //    {
            //        foreach (var o in a.OldItems.OfType<GradientPoint>())
            //        {
            //            values.Remove(o);
            //        }
            //    }
            //};
        }


//        public SeriesCollection Ratios { get; }

    }
}