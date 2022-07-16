using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.HistoricalTools;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Tool;
using HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph.Links;
using HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph;
using HLab.Erp.Lims.Monographs.Module.Graph;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    using H = H<MonographGraphViewModel>;

    public class MonographGraphViewModel : MonographViewModel, IDropViewModel, IMvvmContextProvider
    {
        private readonly IMessageBus _msg;
        private readonly IDataService _db;
        public IMvvmService MvvmService { get; }
        public MonographGraph MonographGraph { get; }


        public sealed class StringWriterWithEncoding : StringWriter
        {
            public StringWriterWithEncoding() { }

            public StringWriterWithEncoding(Encoding encoding)
            {
                Encoding = encoding;
            }

            public override Encoding Encoding { get; }
        }

        public ICommand SaveCommand { get; } = H.Command(c => c
            .Action(
                (self) =>
                {
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Encoding = Encoding.UTF8,
                        NewLineOnAttributes = true,
                        Indent = true
                    };

                    string s = "";

                    using (var sw = new StringWriterWithEncoding(Encoding.UTF8))
                    {
                        using (var xw = XmlWriter.Create(sw, settings))
                        {
                            xw.WriteStartDocument();
                            self.MonographGraph.WriteXml(xw);
                            xw.WriteEndDocument();
                            xw.Close();
                            s = sw.ToString();
                        }
                    }
                    self.Model.GraphSource = s;
                    Clipboard.SetText(s);
                })
        );

        public ICommand LoadCommand { get; } = H.Command(c => c
            .Action(
                (e) =>
                {
                    e.MonographGraph.Monograph = e.Model;
                    e.MonographGraph.Load(e.Model.GraphSource);

                })
        );



        public MonographGraphViewModel(IMessageBus msg)
        {
            _msg = msg;
            _msg.Subscribe<SelectedMonographieEditor>(
                msg => NewSolutionName = msg.Text);

            H.Initialize(this);
        }

        //public override MvvmContext MvvmContext => N.Get(() => this.GetParentMvvmContext().GetNewContext("MonographGraph")
        //        .AddCreator<IGraphViewModel>( (e) => e.Root = this)
        //        .AddCreator<LinkGraphViewModel>((e) => e.Root = this)
        //        .AddCreator<NewSolutionViewModel>((e) => e.Root = this));

        public void ConfigureMvvmContext(IMvvmContext ctx)
        {
            ctx.AddCreator<IGraphViewModel>(e => e.Root = this);
            ctx.AddCreator<LinkGraphViewModel>(e => e.Root = this);
            ctx.AddCreator<NewSolutionViewModel>(e => e.Root = this);
        }

        public string DebugText
        {
            get => _debugText.Get();
            set => _debugText.Set(value);
        }

        private readonly IProperty<string> _debugText = H.Property<string>(c => c.Default(""));


        public int Zoom
        {
            get => _zoom.Get();
            set => _zoom.Set(value);
        }
        private readonly IProperty<int> _zoom = H.Property<int>(c => c.Default(25));

        public double Scale => _scale.Get();
        private readonly IProperty<double> _scale = H.Property<double>(c => c
            .On(e => e.Zoom)
            .Set(e => ((e.Zoom + 50.0) / 150.0) * 2.0)
        );


        public Canvas DragCanvas => null;//TODO IOC ((ApplicationViewModel)((ApplicationServiceWpf)Application).ViewModel).DragCanvas;


        public void Select(INotifyPropertyChanged viewModel)
        {
            bool darken = viewModel != null;

            foreach (var vm in ConsumablesViewModels.OfType<ConsumableGraphViewModel>())
            {
                vm.State.Selected = false;
                vm.State.Darken = darken;
            }

            //foreach (var vm in SolutionsViewModels.OfType<SolutionGraphViewModel>())
            //{
            //    vm.State.Selected = false;
            //    vm.State.Darken = darken;
            //}

            foreach (var vm in TestsViewModels.OfType<TestGraphViewModel>())
            {
                vm.State.Selected = false;
                vm.State.Darken = darken;
            }

            //foreach (var vm in LinksViewModels.OfType<LinkGraphViewModel>())
            //{
            //    vm.State.Selected = false;
            //    vm.State.Darken = darken;
            //}
            if (viewModel is  IGraphViewModel gvm)
            {
                gvm.State.Selected = true;
                gvm.State.Darken = false;
            }
            else if (viewModel is LinkGraphViewModel l)
            {
                l.State.Selected = true;
                l.State.Darken = false;
            }
        }

        //[Import]
        //public ObservableViewModelCollection<LinkGraphViewModel> LinksViewModels
        //{
        //    get => _linksViewModels.Get();
        //    set => _linksViewModels.Set(value.SetViewModeContext(() => MvvmContext)
        //        .SetViewMode<ViewModeDefault>()
        //        .Link(() => Model?.Links));
        //}

        //private readonly IProperty<ObservableViewModelCollection<LinkGraphViewModel>> _linksViewModels = H.Property<ObservableViewModelCollection<LinkGraphViewModel>>(c => c
        //    .On(e => e.Model.Links)
        //    .Update()
        //);

        public ObservableViewModelCollection<ConsumableGraphViewModel> ConsumablesViewModels
        {
            get => _consumablesViewModels.Get();
            set => _consumablesViewModels.Set(value.SetViewModeContext(() => MvvmContext)
                .SetViewMode<ViewModeDefault, IViewClassFlowchart>()
                .Link(() => Model?.Consumables));
        }

        private readonly IProperty<ObservableViewModelCollection<ConsumableGraphViewModel>> _consumablesViewModels = H.Property<ObservableViewModelCollection<ConsumableGraphViewModel>>(c => c
            .On(e => e.Model.Consumables)
            .Do(e => e.ConsumablesViewModels.OnTriggered())
        );

        //[Import]
        //public ObservableViewModelCollection<SolutionGraphViewModel> SolutionsViewModels
        //{
        //    get => _solutionsViewModels.Get();
        //    set => _solutionsViewModels.Set(value.SetViewModeContext(() => MvvmContext)
        //        .SetViewMode<ViewModeDefault, IViewClassFlowchart>()
        //        .Link(() => Model?.Solutions));
        //}

        //private readonly IProperty<ObservableViewModelCollection<SolutionGraphViewModel>> _solutionsViewModels = H.Property<ObservableViewModelCollection<SolutionGraphViewModel>>(c => c
        //    .On(e => e.Model.Solutions)
        //    .Update()
        //);


        public ObservableViewModelCollection<TestGraphViewModel> TestsViewModels
        {
            get => _assaysViewModels.Get();
            set => _assaysViewModels.Set(value.SetViewModeContext(() => MvvmContext)
                .SetViewMode<ViewModeDefault, IViewClassFlowchart>()
                .Link(() => Model?.Tests));
        }

        private readonly IProperty<ObservableViewModelCollection<TestGraphViewModel>> _assaysViewModels = H.Property<ObservableViewModelCollection<TestGraphViewModel>>(c => c
            .On(e => e.Model.Tests)
            .Do(e => e.TestsViewModels.OnTriggered())
        );


        public ObservableQuery<Unit> Units
        {
            get => _units.Get();
            set => _units.Set(value.FluentUpdate());
        }

        private readonly IProperty<ObservableQuery<Unit>> _units = H.Property<ObservableQuery<Unit>>(c => c
            //.On(e => e)
            //.Update()
        );


        public string NewSolutionName
        {
            get => _newSolutionName.Get();
            set => _newSolutionName.Set(value);
        }

        private readonly IProperty<string> _newSolutionName = H.Property<string>(c => c.Default("Nouvelle Solution"));


        //public ICommand NewSolutionCommand { get; } = H.Command(c => c
        //    .Action(
        //        (e) => e.GetOrAddSolution(e.NewSolutionName))
        //);


        public MonographTest AddMonographTest(TestClass TestClass, bool publish = false)
        {
                 return _db.Add<MonographTest>(
                    t =>
                    {
                        t.MonographId = Model.Id;
                        t.TestClassId = TestClass.Id;
                    },
                    m => { if (publish) _msg.Publish(new EntityMessage<Monograph>(Model.Id)); }
                    );
        }

        //public MonographSolution GetOrAddSolution(String designation)
        //{
        //    var id = Model.Id;

        //        return _db.GetOrAdd<MonographSolution>(
        //            s => designation == s.Designation && id == s.MonographId,
        //            s =>
        //            {
        //                s.MonographId = Model.Id;
        //                s.Designation = designation;
        //                s.QtyMode = "f";
        //            },
        //            m =>
        //            {
        //                Model.Solutions.FluentUpdate();
        //            }
        //            );
        //}
        public MonographConsumable GetOrAdd(Consumable consommable)
        {
                return _db.GetOrAdd<MonographConsumable>(
                    cmn => Model.Id == cmn.MonographId && consommable.Id == cmn.ConsumableId,
                    m =>
                    {
                        m.MonographId = Model.Id;
                        m.ConsumableId = consommable.Id;

                    },
                    m => { Model.Consumables.FluentUpdate(); }
                    );
        }
        public MonographTest Add(TestClass TestClass)
        {
                return _db.Add<MonographTest>(
                    m =>
                    {
                        m.MonographId = Model.Id;
                        m.TestClassId = TestClass.Id;
                        m.Name = TestClass.Name;
                        m.Description = "<description>";
                        //m.Comment = testEchantillon.Comment;
                        m.Specifications = "<spécification>";
                        m.Order = TestClass.Order;
                    },
                    m => { Model.Tests.FluentUpdate(); }
                    );
        }

        public MonographTest Add(SampleTest sampleAssay)
        {
            return _db.Add<MonographTest>(
                m =>
                {
                    m.MonographId = Model.Id;
                    m.TestClassId = sampleAssay.TestClassId;
                    m.Name = sampleAssay.TestName;
                    m.Description = sampleAssay.Description;
                    m.Comment = sampleAssay.Note;
                    m.Specifications = sampleAssay.Specification;
                    m.Order = sampleAssay.Order;
                },
                m => { Model.Tests.FluentUpdate(); }
                );
        }


        //internal MonographLink GetOrAddLink(MonographSolution ms, MonographSolution solution)
        //{
        //    if (ms == null) return null;


        //    var link = _db.GetOrAdd<MonographLink>(
        //        ml => Model.Id == ml.MonographId
        //              && ms.Id == ml.MonographSolutionId
        //              && solution.Id == ml.MonographSolutionParentId,
        //        ml =>
        //        {
        //            ml.MonographId = Model.Id;
        //            ml.MonographSolutionId = ms.Id;
        //            ml.MonographSolutionParentId = solution.Id;

        //            ml.SetDefaultValue(ms.UnitGroup);
        //        },
        //        m =>
        //        {
        //            m.UpdateLinks();
        //        }
        //    );
        //    return link;
        //}

        //public MonographLink GetOrAddLink(MonographConsumable mc, MonographSolution solution)
        //{
        //    if (mc == null) return null;
        //    var link = _db.GetOrAdd<MonographLink>(
        //        ml => Model.Id == ml.MonographId
        //              && mc.Id == ml.MonographConsumableId
        //              && solution.Id == ml.MonographSolutionParentId,
        //        ml =>
        //        {
        //            ml.MonographId = Model.Id;
        //            ml.MonographConsumableId = mc.Id;
        //            ml.MonographSolutionParentId = solution.Id;

        //            ml.SetDefaultValue(mc.Consumable.UnitGroup);
        //        },
        //        m =>
        //        {
        //            m.UpdateLinks();
        //        }
        //    );

        //    return link;
        //}


        //public MonographLink GetOrAddLink(MonographTest test, MonographSolution solution, bool publish = false)
        //{
        //        if (test != null)
        //        {
        //            var link = _db.GetOrAdd<MonographLink>(
        //                ml => Model.Id == ml.MonographId
        //                      && test.Id == ml.MonographAssayId
        //                      && solution.Id == ml.MonographSolutionId,
        //                ml =>
        //                {
        //                    ml.MonographId = Model.Id;
        //                    ml.MonographAssayId = test.Id;
        //                    ml.MonographSolutionId = solution.Id;
        //                },
        //                m =>
        //                {
        //                    m.UpdateLinks();
        //                }
        //                );

        //            return link;
        //        }
        //        return null;
            
        //}

        //public MonographLink GetOrAddLink(Consumable consommable, string solutionName)
        //{
        //    return GetOrAddLink(consommable, GetOrAddSolution(solutionName));
        //}

        //public MonographLink GetOrAddLink(Consumable consommable, MonographSolution solution)
        //{
        //    return GetOrAddLink(GetOrAdd(consommable), solution);
        //}

        //public MonographLink GetOrAddLink(SampleTest sampleTest, MonographSolution solution)
        //{
        //    return GetOrAddLink(Add(sampleTest), solution);
        //}

        //public MonographLink GetOrAddLink(TestClass testClass, MonographSolution solution)
        //{
        //    return GetOrAddLink(Add(testClass), solution);
        //}


        public bool DragEnter(ErpDragDrop data)
        {
            if (data.DraggedElement.DataContext is ConsumableDragDropViewModel) return true;
            if (data.DraggedElement.DataContext is SampleAssayDragDropViewModel) return true;
            if (data.DraggedElement.DataContext is TestClassDragDropViewModel) return true;

            return false;
        }

        public bool DragLeave(ErpDragDrop data)
        {
            return true;
        }

        public bool DropIn(ErpDragDrop e)
        {
            if (e.DraggedElement == null) return false;

            if (e.DraggedElement.DataContext is ConsumableDragDropViewModel cm)
            {
                var mc = GetOrAdd(cm.Model);
                Model.Consumables.Selected = mc;
                e.DraggedElement = null;
                return true;
            }

            if (e.DraggedElement.DataContext is SampleAssayDragDropViewModel te)
            {
                var mt = Add(te.Model);
                Model.Tests.Selected = mt;
                e.DraggedElement = null;
                return true;
            }

            if (e.DraggedElement.DataContext is TestClassDragDropViewModel t)
            {
                var mt = Add(t.Model);
                Model.Tests.Selected = mt;
                e.DraggedElement = null;
                return true;
            }

            e.DraggedElement = null;
            return false;
        }

        public void DropOut(ErpDragDrop e)
        {

        }

        public bool DragStart(ErpDragDrop data)
        {
            return true;
        }

    }
}
