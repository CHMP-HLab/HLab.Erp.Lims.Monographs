using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Monographs.Data;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.HistoricalTools;
using HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Tool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.EditorTool;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph;
using HLab.Mvvm;
using HLab.Mvvm.Extensions;
using HLab.Notify.Annotations;
using HLab.Notify.PropertyChanged;
using BlockGraphView = HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph.BlockGraphView;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph
{
    using H = H<SolutionGraphViewModel>;

    public class SolutionGraphViewModel : GraphViewModel<MonographSolution>, IDropViewModel
    {
        private IMessageBus _msg;
        private IDataService _db;

        public SolutionGraphViewModel(IMessageBus msg, IDataService db, IDialogService dialog)
        {
            _msg = msg;
            _db = db;
            _dialog = dialog;
            H.Initialize(this);
            _msg.Subscribe<MonographEditorViewModel>(a =>
            {
                if (a.Monograph?.Id == Model.MonographId) Bind(a);
            });
        }


        private WeakReference<MonographEditorViewModel> _editor = null;

        public void Link()
        {
            if (_editor == null || !_editor.TryGetTarget(out var vm)) return;

            var span = vm.Link(Model.AnchorId(), State, "TextBackground");
            if (span != null) span.MouseLeftButtonDown += Span_MouseLeftButtonDown;
        }


        private void Bind(MonographEditorViewModel vm)
        {
            _editor = new WeakReference<MonographEditorViewModel>(vm);
            var span = vm.Bind(Model.AnchorId(), State, "TextBackground");
            if (span != null) span.MouseLeftButtonDown += Span_MouseLeftButtonDown;
        }


        private void Span_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Selected = true;
        }

        [TriggerOn(nameof(Model), "QtyMode")] public bool QuantiteReadOnly => Model.QtyMode != "f";


        [TriggerOn(nameof(Model), "QtyMode")]
        public Visibility GradientVisibility => Model.QtyMode == "g" ? Visibility.Visible : Visibility.Hidden;


        public Dictionary<string, string> QtyModeList => new Dictionary<string, string>
        {
            {"f", "Volume"},
            {"m", "Masse"},
            {"u", "Unité"},
//            {"a","Auto"},
            {"g", "Injection"},
        };

        [TriggerOn(nameof(Model), "QtyMode")]
        public KeyValuePair<string, string> QtyMode
        {
            get => QtyModeList.FirstOrDefault(e => e.Key == Model.QtyMode);
            set => Model.QtyMode = value.Key;
        }


        //private void StartDragging(ErpDragDrop e)
        //{
        //    if (e.IsDragging)
        //    {
        //        e.Move += DragMove;
        //        e.Drop += DragDrop;
        //    }
        //}

        private void DragDrop(ErpDragDrop e)
        {
            if (e.DraggedElement != null)
            {

                FrameworkElement view = this.GetActualView();

                if (e.HitTest(view))
                {
                    var item = e.DraggedElement.DataContext as ConsumableDragDropViewModel;
                    e.DraggedElement = null;

                    if (item != null)
                    {
                        //Root.GetOrAddLink(item.Model, Model);
                        State.Selected = true;
                    }
                }
            }

            e.Move -= DragMove;
            e.Drop -= DragDrop;
            IsSelectable = true;
        }

        private void DragMove(ErpDragDrop e)
        {
            switch (e.DraggedElement)
            {
                case DragConsumableItemView _:
                {
                    if (!(e.DraggedElement.DataContext is ConsumableDragDropViewModel c)) return;

                    if (true
                        //Model.LeftLinks.Any(
                        //    l =>
                        //        (l.Left as MonographConsumable)?.ConsumableId == c.Model?.Id)
                        )
                        IsSelectable = false;
                    else
                    {
                        IsSelectable = true;

                        var view = this.GetActualView();

                        State.Selected = e.HitTest(view);
                    }

                    break;
                }

                case BlockGraphView _:
                    IsSelectable = true;
                    break;
                default:
                    IsSelectable = false;
                    break;
            }
        }

        public bool IsSelectable
        {
            get => _isSelectable.Get();
            set => _isSelectable.Set(value);
        }

        private readonly IProperty<bool> _isSelectable = H.Property<bool>();



        public override bool IsConnectable(string thisClass, IGraphViewModel model, string modelClass)
        {
            if (model is SolutionGraphViewModel solution)
            {
                if (thisClass == modelClass) return false;
                if (thisClass == "right")
                {
                    if (Model.LeftLinkedTo(solution.Model)) return false;
                }
                else
                {
                    if (Model.RightLinkedTo(solution.Model)) return false;
                }

                return true;
            }

            return model.IsConnectable(modelClass, this, thisClass);
        }

        public override void ConnectTo(string thisClass, IGraphViewModel model, string modelClass)
        {
            if (model is SolutionGraphViewModel solution)
            {
                if (thisClass == modelClass) return;
                if (thisClass == "left")
                {
                    //Root.GetOrAddLink(solution.Model, Model);
                }
                else
                {
                    //Root.GetOrAddLink(Model, solution.Model);
                }
            }
            else
                model.ConnectTo(modelClass, this, thisClass);
        }

        //        private static int test = 0;

        //[TriggerOn(nameof(LeftLinks), "Item", "LeftViewModel", "State", "Color")]
        //public override void SetColor()
        //{
        //    if (LeftLinks == null) return;
        //    float n = 0, sa = 0, sc = 0, sm = 0, sy = 0, sk = 0;

        //    var list = LeftLinks.Where(l => l.LeftViewModel != null).ToList();

        //    foreach (var color in list
        //        .Select(e => e.LeftViewModel.State.Color))
        //    {
        //        var k = 1 - Math.Max(Math.Max(color.ScR, color.ScG), color.ScB);
        //        sc += (1 - color.ScR - k) / (1 - k);
        //        sm += (1 - color.ScG - k) / (1 - k);
        //        sy += (1 - color.ScB - k) / (1 - k);
        //        sk += k;

        //        sa += color.ScA;
        //        n++;
        //    }

        //    sc /= n;
        //    sm /= n;
        //    sy /= n;
        //    sk /= n;
        //    sa /= n;

        //    State.Color = Color.FromScRgb(sa, (1 - sc) * (1 - sk), (1 - sm) * (1 - sk), (1 - sy) * (1 - sk));

        //}

        public void SetColor2()
        {
            //if (test > 6) return;
            //if(test==6)
            //{ }
            //test++;


            float a = 0, r = 0, g = 0, b = 0;
            int n = 0;

            //foreach (var color in LeftLinks.Select(e => e.LeftViewModel?.State.Color))
            //{
            //    if (color == null) continue;
            //    a += color.Value.ScA;
            //    r += color.Value.ScR;
            //    g += color.Value.ScG;
            //    b += color.Value.ScB;
            //    n++;
            //}

            if (n == 0) State.Color = Colors.Gray;
            else
            {
                a /= n;
                r /= n;
                g /= n;
                b /= n;

                State.Color = Color.FromScRgb(a, r, g, b);
            }

//            SetRightColor();
        }

        public override int Order => Model.Order ?? 0;

        //[TriggerOn(nameof(LeftLinks), "Item", "LeftViewModel", "HorizontalOrder")]
        //public override int HorizontalOrder => N.Get(() => LeftLinks.Count == 0)
        //    ? 0
        //    : LeftLinks.Max(e => e.LeftViewModel.HorizontalOrder) + 1;

        //[TriggerOn(nameof(HorizontalOrder))]
        //public override int Column => N.Get(() => 2 * HorizontalOrder);


        //[TriggerOn(nameof(LeftLinks), "Item", "LeftViewModel", "Width")]
        //[TriggerOn(nameof(LeftLinks), "Item", "LeftViewModel", "Left")]
        //public double Left => N.Get(() =>
        //{
        //    var left = LeftLinks.Select(e => e.LeftViewModel).OfType<SolutionGraphViewModel>().ToList();

        //    if (left.Any()) return left.Max(e => e.Left + e.Width) + 100;

        //    return 0.0;
        //});

        //public double Right => _right.Get();

        //private readonly IProperty<double> _right = H.Property<double>(c => c
        //    .On(e => e.RightLinks.Item().RightViewModel.Width)
        //    .On(e => e.RightLinks.Item().RightViewModel.Right)
        //    .On(e => e.Model.Right)
        //    .Set(e =>
        //    {
        //        var rights = e.RightLinks.Select(f => f.RightViewModel).OfType<SolutionGraphViewModel>().ToList();

        //        var right = (rights.Any()) ? rights.Max(f => f.Right + f.Width) + 100 : 0;

        //        return right + e.Model.Right;
        //    })

        //);


        public override double Top
        {
            get => _top.Get();
            set => _top.Set(value);
        }

        private readonly IProperty<double> _top = H.Property<double>(c => c

            .On(self => self.Model.Top)
            .Set(self => self.Model.Top));


        public Thickness Margin => _margin.Get();

        private readonly IProperty<Thickness> _margin = H.Property<Thickness>(c => c
            .On(self => self.Top)
            .On(self => self.Right)
            .Set(self => new Thickness( /*Left*/0, self.Top, self.Right, 0)));


        public override string IconName => _iconName.Get();

        private readonly IProperty<string> _iconName = H.Property<string>(c => c
            .On(self => self.Model.QtyMode)
            .Set(self =>
                {
                    switch (self.Model.QtyMode)
                    {
                        case "g": return "IconSegingue";
                        //                case "g": return "IconSegingue";
                        default: return "IconSolution";
                    }
                }
            ));



        public Visibility RightAnchorVisibility => Visibility.Visible;
        public Visibility LeftAnchorVisibility => Visibility.Visible;



        public double Quantite
        {
            get => _quantite.Get();
            set => _quantite.Set(value);
        }

        private readonly IProperty<double> _quantite = H.Property<double>();

        public double QuantiteSuivant
        {
            get => _quantiteSuivant.Get();
            set => _quantiteSuivant.Set(value);
        }

        private readonly IProperty<double> _quantiteSuivant = H.Property<double>();


        public Unit Unit
        {
            get => _unit.Get();
            set => _unit.Set(value);
        }

        private readonly IProperty<Unit> _unit = H.Property<Unit>();



        public IObservableFilter<Unit> UnitMinList { get; } = H.Filter<Unit>(c => c
                .AddFilter((s,e) => e.UnitClass.Symbol == s.Model?.UnitGroup)
                .Link(e => e.Root?.Units)
                .On(e => e.Root.Units)
                .On(e => e.Model.UnitGroup)
                .Update()
        );


        [TriggerOn(nameof(UnitMinList), "Item")]
        [TriggerOn(nameof(Model), "UnitMinId")]
        public Unit UnitMinEdit
        {
            get => UnitMinList.FirstOrDefault(e => e.Id == Model?.UnitMinId);
            set
            {
                if (Model == null) return;
                Model.UnitMinId = value?.Id;
            }
        }

        public double Tarif
        {
            get => _tarif.Get();
            set => _tarif.Set(value);
        }

        private readonly IProperty<double> _tarif = H.Property<double>();

        public double TarifSuivant
        {
            get => _tarifSuivant.Get();
            set => _tarifSuivant.Set(value);
        }

        private readonly IProperty<double> _tarifSuivant = H.Property<double>();

        private readonly IDialogService _dialog;


        public override ICommand DeleteCommand { get; } = H.Command(c => c
            //.CanExecute(self => (self.RightLinks?.Count ?? 0) == 0 && (self.LeftLinks?.Count ?? 0) == 0)
            .Action(
                (self) =>
                {
                    self.DeleteModel(self._dialog, self._db);
                    //self.Root.Model.Solutions.FluentUpdate();

                }
                )
            //.On(self => self.RightLinks.Item())
            //.On(self => self.LeftLinks.Item())
            .CheckCanExecute()
        );
    


    public bool DragEnter(ErpDragDrop e)
        {
            if (e.DraggedElement == null) return false;

            if (e.DraggedElement.DataContext is ConsumableDragDropViewModel)
            {
                State.Selected = true;
                return true;
            }

            if (e.DraggedElement.DataContext is SampleAssayDragDropViewModel)
            {
                State.Selected = true;
                return true;
            }

            if (e.DraggedElement.DataContext is AssayDragDropViewModel)
            {
                State.Selected = true;
                return true;
            }

            return false;
        }

        public bool DragLeave(ErpDragDrop e)
        {
            State.Selected=false;
            return true;
        }


        public void DropOut(ErpDragDrop data)
        {
            State.Disabled = false;
            IsSelectable = true;
        }

        public bool DropIn(ErpDragDrop e)
        {
            if (e.DraggedElement != null)
            {
                if (e.DraggedElement.DataContext is ConsumableDragDropViewModel cm)
                {
                    //Root.GetOrAddLink(cm.Model, Model);
                    State.Selected = true;
                }

                if (e.DraggedElement.DataContext is SampleAssayDragDropViewModel te)
                {
                    //Root.GetOrAddLink(te.Model, Model);
                    State.Selected = true;
                }

                if (e.DraggedElement.DataContext is AssayDragDropViewModel t)
                {
                    //Root.GetOrAddLink(t.Model, Model);
                    State.Selected = true;
                }

                e.DraggedElement = null;
            }
            DropOut(e);
            IsSelectable = true;
            return true;
        }

        public bool DragStart(ErpDragDrop e)
        {
            if (e.DraggedElement == null)
            {
                IsSelectable = false;
                return false;
            }

            if (e.DraggedElement.DataContext is ConsumableDragDropViewModel cvm)
            {
                if (true
                    //LeftLinks.Any(l =>
                    //    (l.LeftViewModel as ConsumableGraphViewModel)?.Model?.ConsumableId == cvm?.Model.Id)
                    )
                {
                    State.Disabled = true;
                    IsSelectable = false;
                    return false;
                }

                IsSelectable = true;
                return true;
            }

            if (e.DraggedElement.DataContext is SampleAssayDragDropViewModel)
            {
                IsSelectable = true;
                return true;
            }

            IsSelectable = false;
            return false;
        }

        public override void SetColor()
        {
            throw new NotImplementedException();
        }
    }
}
