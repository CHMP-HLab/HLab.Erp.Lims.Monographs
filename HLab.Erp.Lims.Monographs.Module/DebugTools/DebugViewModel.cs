using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HLab.Core.DebugTools;
using HLab.Mvvm;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.DebugTools
{
    using H = H<DebugViewModel>;

    internal class DebugViewModel : ViewModel
    {
        readonly IDebugLogger _log;
        public DebugViewModel(IDebugLogger log)
        {
            _log = log;
            // TODO
            //Series = new SeriesCollection()
            //{
            //    new ColumnSeries()
            //    {
            //        Title = "Deciles",
            //        Values = Values,
                    
            //    }
            //};

            H.Initialize(this);
        }

        public ObservableCollection<string> List => _list.Get();

        readonly IProperty<ObservableCollection<string>> _list = H.Property<ObservableCollection<string>>(c => c
            .Set(e => new ObservableCollection<string>())
        );

        public ObservableCollection<string> Labels => _labels.Get();

        readonly IProperty<ObservableCollection<string>> _labels = H.Property<ObservableCollection<string>>(c => c
            .Set(e => new ObservableCollection<string>())
        );

        public string Selected
        {
            get => _selected.Get();
            set
            {
                if(_selected.Set(value)) Update();
            }
        }

        readonly IProperty<string> _selected = H.Property<string>();

        // TODO
        //public SeriesCollection Series { get; }

        //public ChartValues<long> Values => _values.Get();
        //private readonly IProperty<ChartValues<long>> _values = H.Property<ChartValues<long>>(c => c
        //    .Set(e => new ChartValues<long>())
        //);

        public ICommand UpdateCommand { get; }  = H.Command(c => c
             .Action(
                (self) =>
                {
                    self.List.Clear();
                    foreach (var n in self._log.Timings.OrderByDescending(e => e.Value.SumMillis))
                        self.List.Add(n.Key);
                })
        );


        void Update()
        {
            var list = new long[10];

            if (Selected == null) return;

            var timing = _log.Timings[Selected];
            var ticks = timing.Ticks.OrderBy(e => e);


            long maxticks = ticks.Max();
            double max = 1000 * maxticks / timing.Frequency;

            Labels.Clear();
            for (int i = 0; i < 10; i++)
            {
                Labels.Add($"{i*max/10:N0}");
            }

            foreach(var t in ticks)
            {
                list[(int)(9*t/maxticks)]++;
            }

            // TODO

            //Values.Clear();
            //Values.AddRange(list);
        }
    }
}
