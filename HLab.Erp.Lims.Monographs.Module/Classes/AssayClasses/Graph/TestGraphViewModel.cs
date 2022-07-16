using System.Windows;
using System.Windows.Input;
using HLab.ColorTools.Wpf;
using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Graph;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;

namespace HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Graph
{
    using H = H<TestGraphViewModel>;

    public class TestGraphViewModel : GraphViewModel<MonographTest>
    {
        private readonly IDataService _db;
        private readonly IDialogService _dialog;

        public TestGraphViewModel(IDataService db, IDialogService dialog)
        {
            _db = db;
            _dialog = dialog;
            H.Initialize(this);
        }

        public override bool IsConnectable(string thisClass, IGraphViewModel model, string modelClass)
        {
            if (thisClass != "left") return false;
            if (model is SolutionGraphViewModel solution)
            {
                return false;
                //return !Model.LeftLinks.Any(e => ReferenceEquals(e.Left, solution.Model));
            }
            return false;
        }

        public override void ConnectTo(string thisClass, IGraphViewModel model, string modelClass)
        {
            if (!IsConnectable(thisClass, model, modelClass)) return;
            //Root.GetOrAddLink(Model, ((SolutionGraphViewModel)model).Model, true);
            return;
        }


        public override ICommand DeleteCommand { get; } = H.Command(c => c
            //.CanExecute(self => (self.RightLinks?.Count ?? 0) == 0 && (self.LeftLinks?.Count ?? 0) == 0)
            .Action(
                (self) =>
                {
                    self.DeleteModel(self._dialog, self._db);
                    //self.Root.Model.Assays.FluentUpdate();
                })
            //.On(self => self.RightLinks.Item())
            //.On(self => self.LeftLinks.Item())
            //.CheckCanExecute()
                
        );




        public FrameworkElement ContentViewModel { get; } = new AssayFlowchartView();
        public FrameworkElement RightContentViewModel => null;

        public FrameworkElement LeftContentViewModel
        {
            get => _leftContentViewModel.Get();
            private set => _leftContentViewModel.Set(value);
        }
        private readonly IProperty<FrameworkElement> _leftContentViewModel = H.Property<FrameworkElement>(c => c.Default((FrameworkElement)default));


        public override string IconName => _iconName.Get();
        private readonly IProperty<string> _iconName = H.Property<string>(c => c
            .On(e => e.Model.TestClass.IconPath)
            .Set(e => "Assays/" + e.Model?.TestClass?.IconPath ?? "IconSolution"));


        [TriggerOn(nameof(Model), "TestClass", "Color")]
        public override void SetColor()
        {
            if (Model == null) return;
            State.Color = Model.TestClass.Color.ToColor();
        }


        public bool IsSelectable { get; } = true;

        public Visibility RightAnchorVisibility => Visibility.Hidden;
        public Visibility LeftAnchorVisibility => Visibility.Visible;


        public Unit Unit { get; set; }




        [TriggerOn(nameof(Model), "Specifications")]
        public string Specification
        {
            get => Model.Specifications; set
            {
                string v = value;

                //v = v.Replace(">=", "≥");
                //v = v.Replace("<=", "≤");
                //v = v.Replace("+-", "±");
                //v = v.Replace("!=", "≠");
                //v = v.Replace("~=", "≈");
                //v = v.Replace("/8", "∞");
                Model.Specifications = v;
            }
        }
    }
}
