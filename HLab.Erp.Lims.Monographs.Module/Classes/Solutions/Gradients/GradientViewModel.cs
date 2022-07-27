using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

using HLab.Base.Extensions;
using HLab.Erp.Data;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Gradients
{
    using H = H<GradientViewModel>;

    public class GradientViewModel : ViewModel<Gradient>, IMvvmContextProvider
    {
        readonly IDataService _db;

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
            ctx.AddCreator<GradientLineViewModel>(m => { m.Parent = this; });
        }

        public GradientViewModel(IDataService db)
        {
            _db = db;
            //TODO 
            //var config = Mappers.Xy<GradientLineViewModel>()
            //    .Y(g => g.RatioPCent)
            //    .X(g => g.Time);

            //Series = new LineSeries<>
            //{
            //    Title = "Gradients",
            //    Values = Values,
            //    LineSmoothness = 0
            //};

            //Ratios = new SeriesCollection(config) { Series };

            H.Initialize(this);
        }


        // TODO
        [TriggerOn("Values","Item","Time")]
        public void UpdateOrder()
        {
            //var done = false;
            //while (!done)
            //{
            //    done = true;
            //    for (var i = 0; i < Values.Count - 1; i++)
            //    {
            //        if (Values[i].Time <= Values[i + 1].Time) continue;

            //        var v = Values[i];
            //        Values.RemoveAt(i);
            //        Values.Insert(i + 1, v);
            //        done = false;
            //    }
            //}
        }

        // TODO
        //public ChartValues<GradientLineViewModel> Values { get; } = new ChartValues<GradientLineViewModel>();
        //public LineSeries Series { get; }
        //public SeriesCollection Ratios { get; }

        public List<double?> FlowRateList => 
            _db.FetchWhereAsync<Gradient>(e => e.FlowRate!=null).ToListAsync().Result.Select(e => e.FlowRate).Distinct().OrderBy(e => e).ToList();


        public ObservableViewModelCollection<GradientLineViewModel> LinesViewModel
        {
            get => _linesViewModel.Get();
            set => _linesViewModel.Set(value.SetViewModeContext(() => MvvmContext)
                .SetViewMode<ViewModeDefault>()
                .SetObserver(Lines_CollectionChanged)
                .Link(() => Model?.Lines));
        }

        readonly IProperty<ObservableViewModelCollection<GradientLineViewModel>> _linesViewModel = H.Property<ObservableViewModelCollection<GradientLineViewModel>>(c => c
            .On(e => e.Model.Lines)
            .Do(e => e.LinesViewModel.OnTriggered())
        );



        public List<int> NbInjectionFirstList => _nbInjectionFirstList.Get();

        readonly IProperty<List<int>> _nbInjectionFirstList = H.Property<List<int>>(c => c
            .Set(e => e._db.FetchAsync<Gradient>().ToListAsync().Result.Select(i => i.NbInjectionFirst ?? 0).Distinct().OrderBy(i => i).ToList())
        );

        public List<int> NbInjectionNextList => _nbInjectionNextList.Get();

        readonly IProperty<List<int>> _nbInjectionNextList = H.Property<List<int>>(c => c
            .Set(e => e._db.FetchAsync<Gradient>().ToListAsync().Result.Select(i => i.NbInjectionNext ?? 0).Distinct().OrderBy(i => i).ToList())
        );

        public List<double> LostVolumeList => _lostVolumeList.Get();

        readonly IProperty<List<double>> _lostVolumeList = H.Property<List<double>>(c => c
            .Set(e => e._db.FetchAsync<Gradient>().ToListAsync().Result.Select(i => i.LostVolume ?? 0).Distinct().OrderBy(i => i).ToList())
        );

        public List<double?> RatioList => _ratioList.Get();

        readonly IProperty<List<double?>> _ratioList = H.Property<List<double?>>(c => c
            .Set(e => e._db.FetchWhereAsync<GradientLine>(i => i.Ratio != null).ToListAsync().Result.Select(i => i.Ratio).Distinct().OrderBy(i => i).ToList())
        );


        void Lines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                if (e.NewItems != null)
                    foreach (var item in e.NewItems.OfType<GradientLineViewModel>())
                    {
                        //TODO
                        //if(Values.Contains(item)) continue; // todo : ça devrait pas arriver, mais ça arrive

                        //var i = 0;
                        //while (i < Values.Count && Values[i].Time <= item.Time)
                        //    i++;

                        //if (i == Values.Count)
                        //    Values.Add(item);
                        //else
                        //{
                        //    Values.Insert(i, item);
                        //}
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                        foreach (var item in e.OldItems.OfType<GradientLineViewModel>())
                        {
//TODO                            Values.Remove(item);
                        }
                    break;
            }

        }

        public ICommand AddCommand { get; } = H.Command(c => c
            .Action(
                (self) =>
                {
                    self._db.Add<GradientLine>(
                        l =>
                        {
                            l.GradientId = self.Model.Id;
                            if (self.Model.Lines.Count == 0)
                            {
                                l.Time = 0;
                                l.Ratio = 1;
                            }
                            else
                            {
                                var g1 = self.LinesViewModel.Selected ?? self.LinesViewModel.Last();
                                GradientLineViewModel g2 = null;
                                var n = self.LinesViewModel.IndexOf(g1);
                                if (n < self.LinesViewModel.Count() - 1)
                                {
                                    g2 = self.LinesViewModel[n + 1];
                                }

                                l.Ratio = (g1.Model.Ratio + (g2?.Model.Ratio ?? g1.Model.Ratio)) / 2;
                                l.Time = (g1.Model.Time + (g2?.Model.Time ?? (g1.Model.Time + 20))) / 2;
                            }

                        }
                            ,

                            l =>
                            {
                                self.Model.Lines.OnTriggered();
                                self.LinesViewModel.Select((GradientLineViewModel)self.LinesViewModel.MvvmContext.GetLinked<ViewMode>(l));
                            }
                        );
                })
        );


        public ICommand RemoveCommand { get; } = H.Command(c => c
            .CanExecute(self => self.LinesViewModel?.Selected != null)
            .Action(
                (self) =>
                {
                    self._db.Delete(
                        self.LinesViewModel.Selected.Model,

                        tf =>
                        {
                            self.LinesViewModel.Selected = null;
                            self.Model.Lines.OnTriggered();
                        });
                })
            .On(self => self.LinesViewModel.Selected).CheckCanExecute()
        );
    }
}
