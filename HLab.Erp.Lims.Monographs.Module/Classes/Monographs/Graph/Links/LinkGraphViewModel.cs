using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using HLab.Base.Extentions;
using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Core.ViewModelStates;
using HLab.Erp.Data;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph.Links
{
    using H = H<LinkGraphViewModel>;

    public class LinkGraphViewModel : ViewModel<MonographLink>
    {
        readonly IMessageBus _msg;
        readonly IDataService _db;
        public LinkGraphViewModel(IMessageBus msg, IDataService db, IDialogService dialog)
        {
            _msg = msg;
            _db = db;
            _dialog = dialog;
            _msg.Subscribe<MonographEditorViewModel>(a =>
            {
                if (a.Monograph?.Id == Model.MonographId) Bind(a);
            });

            H.Initialize(this);
        }

        public MonographGraphViewModel Root
        {
            get => _root.Get();
            set => _root.Set(value);
        }

        readonly IProperty<MonographGraphViewModel> _root = H.Property<MonographGraphViewModel>();


        WeakReference<MonographEditorViewModel> _editor = null;

        public void Link()
        {
            if (_editor == null || !_editor.TryGetTarget(out var vm)) return;

            var span = vm.Link(Model.AnchorId(),State, "TextBackground");
            if (span != null) span.MouseLeftButtonDown += Span_MouseLeftButtonDown;
        }


        void Bind(MonographEditorViewModel vm)
        {
            _editor = new WeakReference<MonographEditorViewModel>(vm);
            var span = vm.Bind(Model.AnchorId(), State, "TextBackground");
            if (span != null) span.MouseLeftButtonDown += Span_MouseLeftButtonDown;
        }

        void Span_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Selected = true;
        }

        [TriggerOn(nameof(State),"Selected")]
        public bool Selected
        {
            get => State.Selected;
            set
            {
                if (State.Selected != value)
                    Root.Select(value ? this : null);
            }
        }

        public State State { get; set; }
    



        //[TriggerOn(nameof(LeftViewModel),"State","Highlighted")]
        //[TriggerOn(nameof(LeftViewModel),"State","LeftHighlighted")]
        //[TriggerOn(nameof(LeftViewModel),"State","Selected")]
        //protected void SetLeftHighlighted() => State.LeftHighlighted = 
        //    (LeftViewModel?.State.Selected ?? false) || (LeftViewModel?.State.LeftHighlighted ?? false);

        //[TriggerOn(nameof(RightViewModel), "State", "Highlighted")]
        //[TriggerOn(nameof(RightViewModel), "State", "RightHighlighted")]
        //[TriggerOn(nameof(RightViewModel), "State", "Selected")]
        //protected void SetRightHighlighted() => State.RightHighlighted = 
        //    (RightViewModel?.State.Selected ?? false) || (RightViewModel?.State.RightHighlighted ?? false);

        //public IGraphViewModel RightViewModel => _rightViewModel.Get();
        //private readonly IProperty<IGraphViewModel> _rightViewModel = H.Property<IGraphViewModel>(c => c
        //    .On(e => e.Model.Right)
        //    .Set(e =>
        //    {
        //        if (e.Model?.Right == null) return null;
        //        return (IGraphViewModel) e.MvvmContext.GetLinked<ViewModeDefault,IViewClassFlowchart>(e.Model.Right);
        //    })
        //);


        //public IGraphViewModel LeftViewModel => _leftViewModel.Get();
        //private readonly IProperty<IGraphViewModel> _leftViewModel = H.Property<IGraphViewModel>(c => c
        //    .On(e => e.Model.Left)
        //    .Set(e =>
        //    {
        //        if (e.Model?.Left == null) return null;
        //        var lvm = (IGraphViewModel) e.MvvmContext.GetLinked<ViewModeDefault, IViewClassFlowchart>(e.Model.Left);
        //        lvm.SetColor();
        //        e.State.Color = lvm.State.Color;
        //        return lvm;
        //    })
        //);



        [TriggerOn(nameof(Model),"Qty")]
        public double QtyEdit
        {
            get => Model?.Qty??0; set
            {
                if (Model == null) return;
                if (Math.Abs(Model.Qty - Model.QtyNext) < double.Epsilon)
                {
                    Model.QtyNext = value;
                }
                Model.Qty = value;
            }
        }

        [TriggerOn(nameof(Model),"QtyNext")]
        public double QtyNextEdit
        {
            get => Model?.QtyNext??0;
            set
            {
                if (Model == null) return;
                Model.QtyNext = value;
            }
        }



        //[TriggerOn(nameof(Model),"Unit", "Group")]
        //public Visibility QtyVisibility
        //    => (
        //    Model?.LeftUnit?.Group == "qs" 
        //    ) ? Visibility.Collapsed : Visibility.Visible;


        //[TriggerOn("Model.Unit.Group")]
        //public Visibility QtyNextVisibility
        //    => (
        //    Model?.LeftUnit?.Group == "qs" 
        //    ) ? Visibility.Collapsed : Visibility.Visible;


        //[TriggerOn("Model.Unit.Group")]
        //public Visibility RatioVisibility
        //    => Model?.LeftUnit?.IsRatio??false ? Visibility.Collapsed : Visibility.Visible;


        //// TODO : 
        //[TriggerOn(nameof(LeftViewModel), "State", "Color")]
        //public void SetColor() => State.Color = LeftViewModel?.State.Color??new Color();


        //[TriggerOn(nameof(Model),"Unit")]
        //public Unit UnitEdit
        //{
        //    get => UnitList.FirstOrDefault(e=>e.Id == Model?.UnitId);
        //    set
        //    {
        //        if (Model == null) return;
        //        Model.UnitId = value?.Id;
        //        Model.SetDefaultRatio();
        //    }
        //}




        //[TriggerOn(nameof(UnitList) + ".Item")]
        //[TriggerOn(nameof(Model) + ".UnitRatioId")]
        //public Unit UnitRatioEdit
        //{
        //    get => UnitRatioList.FirstOrDefault(e => e.Id == Model?.UnitRatioId);
        //    set
        //    {
        //        if (Model == null) return;
        //        Model.UnitRatioId = value?.Id;
        //    }
        //}

        //[Import]
        //[TriggerOn(nameof(Root), "Units")]
        //[TriggerOn(nameof(Model), "UnitGroup")]
        //[TriggerOn(nameof(Model), "MolarMass")]
        ////[TriggerOn(nameof(Model), "Right", "MainQtyAbs")]
        ////[TriggerOn(nameof(Model), "Right", "QtyAbs")]
        //[TriggerOn(nameof(Model), "Right", "UnitGroup")]
        //private IObservableFilter<Unit> UnitList { get; } = H.Filter<Unit>((e, of) => of
        //    .AddFilter(
        //        f => f.Group == e.Model?.UnitGroup
        //             || (f.Group == "mol" && e.Model?.MolarMass != null)
        //             || (f.Group == "qs" /*&& Entity?.Right.MainQtyAbs<Entity?.Right.QtyAbs*/)
        //             || (f.Group == "vol" && e.Model?.Right?.UnitGroup == "v")
        //             || (f.Group == "pc" && (e.Model?.Right?.UnitGroup == "v" || e.Model?.Right?.UnitGroup == "m"))
        //    ).Link(() => e.Root?.Units));

        //[TriggerOn(nameof(Root), "Units")]
        //[TriggerOn(nameof(Model), "Right", "UnitGroup")]
        //public IObservableFilter<Unit> UnitRatioList { get; } = H.Filter<Unit>((e, of) => of.AddFilter(
        //    f => f.Group == e.Model?.Right?.UnitGroup
        //    //|| (e.Group == "mol" && Entity.MolarMass != null)
        //    //|| (e.Group == "p" && Entity.Left.UnitGroup == "v")
        //    //|| e.Group == "qs"
        //).Link(() => e.Root?.Units));

        //public Unit ViewUnit => _viewUnit.Get();
        //private readonly IProperty<Unit> _viewUnit = H.Property<Unit>(c => c
        //    .On(e => e.Root.Units.Item().Group)
        //    .On(e => e.Model.QtyAbs)
        //    .On(e => e.Model.QtyAbsNext)
        //    .Set(e => e.Root.Units.Where(f => f.Group == e.Model?.UnitGroup)
        //    .BestMatch(Math.Max(e.Model?.QtyAbs??0, e.Model?.QtyAbsNext??0))));



        //public double ViewQty => _viewQty.Get();
        //private readonly IProperty<double> _viewQty = H.Property<double>(c => c
        //    .On(e => e.ViewUnit)
        //    .On(e => e.Model.QtyAbs)
        //    .Set(e => e.ViewUnit?.Qty(e.Model.QtyAbs) ?? 0));

        //public double ViewQtyNext => _viewQtyNext.Get();
        //private readonly IProperty<double> _viewQtyNext = H.Property<double>(c => c
        //    .On(e => e.ViewUnit)
        //    .On(e => e.Model.QtyAbsNext)
        //    .Set(e => e.ViewUnit?.Qty(e.Model.QtyAbsNext) ?? 0));


        readonly IDialogService _dialog;

        public ICommand DeleteCommand { get; } = H.Command(c => c
            .Action(
                (self) => {             
                    self.DeleteModel(self._dialog,self._db);
                    //self.Root?.Model?.Links.FluentUpdate();
})
        );


        public double StrokeThicknessBase => _strokeThicknessBase.Get();

        readonly IProperty<double> _strokeThicknessBase = H.Property<double>(c => c
            //.On(e => e.Model.Cost)
            //.On(e => e.Model.Monograph.ConsumablesCost)
            //.Set(e => e.Model.Monograph.ConsumablesCost > 0 && (e.Model.Cost).IsRegular()?(50 * e.Model.Cost / e.Model.Monograph.ConsumablesCost):0)
        );




        public double StrokeThickness => _strokeThickness.Get();

        readonly IProperty<double> _strokeThickness = H.Property<double>(c => c
            .On(e => e.StrokeThicknessBase)
            .Set(e => Math.Max(3, e.StrokeThicknessBase.IsRegular()?e.StrokeThicknessBase:0))
        );


        public DoubleCollection StrokeDashArray => _strokeDashArray.Get();

        readonly IProperty<DoubleCollection> _strokeDashArray = H.Property<DoubleCollection>(c => c
            .On(e => e.StrokeThicknessBase)
            .Set(e =>  e.StrokeThicknessBase>5? new DoubleCollection(){1,0} : new DoubleCollection(){Math.Max(0.5, e.StrokeThicknessBase*3),0.5 })
        );


        public Visibility LabelVisibility => _labelVisibility.Get();

        readonly IProperty<Visibility> _labelVisibility = H.Property<Visibility>(c => c
            .On(e => e.Selected)
            .Set(e => e.Selected ? Visibility.Visible : Visibility.Collapsed)
        );

    }
}
