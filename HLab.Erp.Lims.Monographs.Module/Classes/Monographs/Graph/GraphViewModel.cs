using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Core.ViewModelStates;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Icons.Annotations.Icons;
using HLab.Mvvm;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;


namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{

    public abstract class GraphViewModel<T> : ViewModel<T>, IGraphViewModel
    {
        protected GraphViewModel() => H<GraphViewModel<T>>.Initialize(this);

        readonly IMessagesService _messageBus;
        readonly IDragDropService _dragDropService;

        protected GraphViewModel(IMessagesService messageBus, IDragDropService dragDrop)
        {
            _messageBus = messageBus;
            _dragDropService = dragDrop;

        }

        public MonographGraphViewModel Root
        {
            get => _root.Get();
            set => _root.Set(value);
        }

        readonly IProperty<MonographGraphViewModel> _root = H<GraphViewModel<T>>.Property<MonographGraphViewModel>();



        public virtual int Order => 0;

        public State State
        {
            get => _state.Get();
            set => _state.Set(value);
        }

        readonly IProperty<State> _state = H<GraphViewModel<T>>.Property<State>();


        //public virtual int HorizontalOrder => -1;
    //public virtual int Column => 0;
    public virtual double Top
    {
        get => _top.Get();
        set => _top.Set(value);
    }

    readonly IProperty<double> _top = H<GraphViewModel<T>>.Property<double>(c => c.Default(0.0));



        public double Width
        {
            get => _width.Get();
            set => _width.Set(value);
        }

        readonly IProperty<double> _width = H<GraphViewModel<T>>.Property<double>(c => c.Default(0.0));



        public virtual double Right
        {
            get => _right.Get();
            //set => _right.Set(value);
        }

        readonly IProperty<double> _right = H<GraphViewModel<T>>.Property<double>(c => c.Default(0.0));


        public IIconService IconService { get; set;}

        public virtual string IconName => "";

        //[Import]
        //public ObservableFilter<LinkGraphViewModel> SelectedLeftLinks
        //{
        //    get => _selectedLeftLinks.Get();
        //    private set => _selectedLeftLinks.Set(value
        //        .AddFilter(l => l.LeftViewModel.State.Selected)
        //        .Link(() => LeftLinks));
        //}

        //private readonly IProperty<ObservableFilter<LinkGraphViewModel>> _selectedLeftLinks =
        //    H.Property<ObservableFilter<LinkGraphViewModel>>();

        //[Import]
        //public ObservableFilter<LinkGraphViewModel> SelectedRightLinks
        //{
        //    get => _selectedRightLinks.Get();
        //    private set => _selectedRightLinks.Set(value
        //        .AddFilter(l => l.RightViewModel.State.Selected)
        //        .Link(() => RightLinks));
        //}
        //private readonly IProperty<ObservableFilter<LinkGraphViewModel>> _selectedRightLinks =
        //    H.Property<ObservableFilter<LinkGraphViewModel>>();

        //public LinkGraphViewModel LeftLink => _leftLink.Get();

        //private readonly IProperty<LinkGraphViewModel> _leftLink = H.Property<LinkGraphViewModel>(c => c
        //    .On(e => e.State.Selected)
        //    .On(e => e.LeftLinks.Item().LeftViewModel.State.Selected)
        //    .Set(e =>
        //    {
        //        var links = e.LeftLinks.Where(l => l.LeftViewModel.State.Selected).ToList();
        //        return links.Count == 1 && !e.State.Selected ? links.FirstOrDefault() : null;
        //    })
        
        //);



        //public LinkGraphViewModel RightLink => _rightLink.Get();
        //private readonly IProperty<LinkGraphViewModel> _rightLink = H.Property<LinkGraphViewModel>(c => c
        //    .On(e => e.State.Selected)
        //    .On(e => e.RightLinks.Item().RightViewModel.State.Selected)
        //    .Set(e =>
        //        {
        //            var links = e.RightLinks.Where(l => l.RightViewModel.State.Selected).ToList();
        //            return (links.Count() == 1 && !e.State.Selected) ? links.FirstOrDefault() : null;
        //        }
        //    ));


        public Panel DragCanvas => _dragCanvas.Get();
        readonly IProperty<Panel> _dragCanvas = H<GraphViewModel<T>>.Property<Panel>(c => c.Set(e => e._dragDropService.GetDragCanvas()));


        public abstract bool  IsConnectable(string thisAnchorClass, IGraphViewModel model, string anchorClass);
        public abstract void ConnectTo(string srcClass, IGraphViewModel dstViewModel, string dstClass);


        public bool Selected
        {
            get => _selected.Get();
            set
            {
                if (State.Selected != value)
                    Root.Select(value ? this : null);
            }
        }

        readonly IProperty<bool> _selected = H<GraphViewModel<T>>.Property<bool>(c => c
            .On(e => e.State.Selected)
            .Set(e => e.State.Selected)
        );

            [TriggerOn(nameof(State), "Selected")]
        public void UpdateSelected()
        {
            if (!State.Selected) return;
            if (Root != null)
            {
                _messageBus.Publish(new DetailMessage(this.Model));
            }
        }

        //private void LeftLinks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems!=null)
        //        foreach (var item in e.NewItems.OfType<LinkGraphViewModel>())
        //        {
        //            item.SetColor();
        //        }
        //}
        //private void RightLinks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //        foreach (var item in e.NewItems.OfType<LinkGraphViewModel>())
        //        {
        //            item.SetColor();
        //        }
        //}
        //[Import]
        //[TriggerOn(nameof(Root), "LinksViewModels","Item","RightViewModel")]
        //public ObservableFilter<LinkGraphViewModel> LeftLinks
        //{
        //    get => _leftLinks.Get();
        //    private set => _leftLinks.Set(value.AddFilter(e =>
        //                e.Model != null
        //                && ReferenceEquals(e.Model.Right, LinkedElement)
        //            )
        //            .Link(() => Root?.LinksViewModels));
        //}

        //private readonly IProperty<ObservableFilter<LinkGraphViewModel>> _leftLinks =
        //    H.Property<ObservableFilter<LinkGraphViewModel>>(c => c
        //        .On(e => e.Root.LinksViewModels.Item().RightViewModel)
        //        .Update()
        //        );

        [TriggerOn]
        public abstract void SetColor();

        //[Import]
        //[TriggerOn(nameof(Root), "LinksViewModels", "Item", "LeftViewModel")]
        //public ObservableFilter<LinkGraphViewModel> RightLinks
        //{
        //    get => _rightinks.Get();
        //    private set => _rightinks.Set(value.AddFilter(
        //                e =>
        //                    e.Model != null
        //                    && ReferenceEquals(e.Model.Left, LinkedElement)
        //            )
        //            .Link(() => Root?.LinksViewModels));
        //}
        //private readonly IProperty<ObservableFilter<LinkGraphViewModel>> _rightinks =
        //    H.Property<ObservableFilter<LinkGraphViewModel>>(c => c
        //        .On(e => e.Root.LinksViewModels.Item().RightViewModel)
        //        .Update()
        //    );

        //[TriggerOn(nameof(LeftLinks), "Item", "State", "LeftHighlighted")]
        //[TriggerOn(nameof(LeftLinks), "Item", "State", "Selected")]
        //protected void SetLeftHighlighted()
        //{
        //    State.LeftHighlighted = LeftLinks.Any(e => e.State.LeftHighlighted || e.State.Selected);
        //}

        //[TriggerOn(nameof(RightLinks), "Item", "State", "RightHighlighted")]
        //[TriggerOn(nameof(RightLinks), "Item", "State", "Selected")]
        //protected void SetRightHighlighted()
        //{
        //    State.RightHighlighted = RightLinks.Any(e => e.State.RightHighlighted || e.State.Selected);
        //}

        public abstract ICommand DeleteCommand { get; }


//        public abstract Color Color { get; }


        public IMonographLinkedElement LinkedElement => _linkedElement.Get();

        readonly IProperty<IMonographLinkedElement> _linkedElement = H<GraphViewModel<T>>.Property<IMonographLinkedElement>(c => c
            .On(e => e.Model)
            .Set(e => e.Model as IMonographLinkedElement)
        );



        public Unit ViewUnit => _viewUnit.Get();

        readonly IProperty<Unit> _viewUnit = H<GraphViewModel<T>>.Property<Unit>(c => c
            .On(e => e.Root.Units.Item().UnitClass.Symbol)
            .On(e => e.LinkedElement.UnitGroup)
            .On(e => e.LinkedElement.QtyAbs)
            .On(e => e.LinkedElement.QtyAbsNext)
            .Set(e => e.Root.Units.Where(f => f.UnitClass.Symbol == e.LinkedElement.UnitGroup).BestMatch(Math.Max(e.LinkedElement.QtyAbs, e.LinkedElement.QtyAbsNext)))
        );


        //DbService.FetchWhere<Unit>()?.BestMatch();
        public double ViewQty => _viewQty.Get();

        readonly IProperty<double> _viewQty = H<GraphViewModel<T>>.Property<double>(c => c
            .On(e => e.ViewUnit)
            .On(e => e.LinkedElement.QtyAbs)
            .Set(e => e.ViewUnit?.Qty(e.LinkedElement.QtyAbs) ?? 0)
        );


        public double ViewQtyNext => _viewQtyNext.Get();

        readonly IProperty<double> _viewQtyNext = H<GraphViewModel<T>>.Property<double>(c => c
            .On(e => e.ViewUnit)
            .On(e => e.LinkedElement.QtyAbsNext)
            .Set(e => e.ViewUnit?.Qty(e.LinkedElement.QtyAbsNext) ?? 0)
        );

    }
}