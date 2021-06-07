using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using HLab.Base.Extensions;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Lims.Monographs.Module.Classes.AssayClasses.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph.Links;
using HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph;
using HLab.Erp.Lims.Monographs.Module.Classes.Suppliers.Graph;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour MonographGraphViewContent.xaml
    /// </summary>
    public partial class MonographFlowchartView : UserControl
        , IView<ViewModeDefault,MonographGraphViewModel>
        , IViewClassFlowchart
    {
        public event RoutedEventHandler ViewLoaded;
        public MonographFlowchartView(Func<Panel,FrameworkElement,ErpDragDrop> getDragDrop)
        {
            InitializeComponent();

            // TODO : new to ioc
            var dragSolution = getDragDrop(GridDragPanelSolutions, PanelSolutions);
            dragSolution.Start += DragSolution_Start;
            dragSolution.Drop += DragSolution_Drop;
            dragSolution.Move += DragSolution_Move;

            Loaded += OnLoaded; //MonographieGraph_DataContextChanged;
            DataContextChanged += MonographGraphContentView_DataContextChanged;
        }

        private void MonographGraphContentView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MonographGraphViewModel vmold)
            {
                vmold.ConsumablesViewModels.CollectionChanged -= ConsumablesViewModels_CollectionChanged;
                //vmold.SolutionsViewModels.CollectionChanged -= SolutionsViewModels_CollectionChanged;
                //vmold.LinksViewModels.CollectionChanged -= LinksViewModels_CollectionChanged;
                vmold.TestsViewModels.CollectionChanged -= TestsViewModels_CollectionChanged;
            }
            if (e.NewValue is MonographGraphViewModel vmnew)
            {
                vmnew.ConsumablesViewModels.SetObserver(ConsumablesViewModels_CollectionChanged);
                //vmnew.SolutionsViewModels.SetObserver(SolutionsViewModels_CollectionChanged); ;
                //vmnew.LinksViewModels.SetObserver(LinksViewModels_CollectionChanged);
                vmnew.TestsViewModels.SetObserver(TestsViewModels_CollectionChanged);
            }
            else
            {
                Debug.Assert(DataContext == null);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // if (DataContext is MonographGraphViewModel vmnew)
            //{
            //    vmnew.ConsumablesViewModels.SetObserver(ConsumablesViewModels_CollectionChanged);
            //    vmnew.SolutionsViewModels.SetObserver(SolutionsViewModels_CollectionChanged); ;
            //    vmnew.LinksViewModels.SetObserver(LinksViewModels_CollectionChanged);
            //    vmnew.TestsViewModels.SetObserver(TestsViewModels_CollectionChanged);
            //}
            //else
            //{
            //    Debug.Assert(DataContext == null);
            //}
        }


        private void TestsViewModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveOldItems(e.OldItems, PanelTests);
            AddNewItems(e.NewItems, PanelTests);
        }

        private void SolutionsViewModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveOldItems(e.OldItems, PanelSolutions);
            AddNewItems(e.NewItems, PanelSolutions);
        }

        private void ConsumablesViewModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveOldItems(e.OldItems, PanelConsumables);
            AddNewItems(e.NewItems, PanelConsumables);
        }

        public IMvvmService Mvvm => (DataContext as MonographGraphViewModel).MvvmService;

        private void LinksViewModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemoveOldItems(e.OldItems, PanelLinks);
            //            AddNewItems(e.NewItems, PanelLinks);

            if (e.NewItems != null)
                foreach (var vm in e.NewItems.OfType<LinkGraphViewModel>())
                {
                    var view = (FrameworkElement)Mvvm.ViewHelperFactory.Get(this).Context.GetView<ViewModeDefault, IViewClassFlowchart>(vm);
                    if (view.Parent == null)
                        PanelLinks.Children.Add(view);
                    else
                    {

                    }
                    //vm.SetSize();
                }
        }

        private static void RemoveOldItems(IEnumerable oldItems, Panel panel)
        {
            if (oldItems == null) return;

            var items = oldItems.Cast<object>().ToList();

            foreach (var cvm in items)
            {
                //remove old items from panel 
                foreach (var fe in
                    panel.Children.OfType<FrameworkElement>()
                        .Where(ee => ReferenceEquals(ee.DataContext, cvm)).ToList())
                {
                    panel.Children.Remove(fe);
                }

                //and from child panels
                foreach (var fe in
                    panel.Children.OfType<ContentControl>().ToList())
                {
                    if (fe.Content is Panel p)
                    {
                        RemoveOldItems(items, p);
                        if (p.Children.Count == 0)
                            panel.Children.Remove(fe);
                    }
                }
            }
        }

        private void AddNewItems(IEnumerable newItems, Panel panel)
        {
            if (newItems == null) return;
            Debug.Assert(panel != null);

            foreach (var vm in newItems.OfType<IGraphViewModel>().OrderBy(e => e.Order).ToList())
            {
                //cvm.Color = GetColor();

                var subPanel = panel;

                if (vm is ConsumableGraphViewModel cvm)
                {
                    ContentControl fv = null;
                    var supplier = cvm.Model.ActualSupplierPrice?.Supplier;
                    if (supplier != null)
                    {
                        foreach (
                            var item in
                            panel.Children.OfType<ContentControl>()
                                .ToList())
                        {
                            if (item.DataContext is SupplierGraphViewModel fvm && fvm.Model.Id == supplier.Id)
                            {
                                fv = item;
                                break;
                            }
                        }

                        if (fv == null)
                        {
                            var ctx = this.GetValue(ViewLocator.MvvmContextProperty) as MvvmContext;
                            fv = ctx?.Mvvm.ViewHelperFactory.Get(this).Context.GetView<ViewModeDefault,IViewClassGraphContainer>(supplier) as ContentControl;

                        }
                        Debug.Assert(fv != null);

                        if (fv.Parent == null)
                        {
                            panel.Children.Add(fv);

                            subPanel = new StackPanel();
                            fv.Content = subPanel;
                        }
                        else
                        {
                            subPanel = fv.Content as StackPanel;
                        }
                    }
                }

                if (vm is TestGraphViewModel tvm)
                {
                    ContentControl tv = null;
                    var t = tvm.Model.TestClass;
                    if (t != null)
                    {
                        foreach (
                            var item in
                            panel.Children.OfType<ContentControl>()
                                .ToList())
                        {
                            if (item.DataContext is TestClassGraphViewModel ttvm && ttvm.Model.Id == t.Id)
                            {
                                tv = item;
                                break;
                            }
                        }

                        if (tv == null)
                        {
                            tv = Mvvm.ViewHelperFactory.Get(this).Context.GetView<ViewModeDefault, IViewClassGraphContainer>(t) as ContentControl;
                            if (tv != null)
                            {
                                panel.Children.Add(tv);

                                subPanel = new StackPanel();
                                tv.Content = subPanel;
                            }
                        }
                        else
                        {
                            subPanel = tv.Content as StackPanel;
                        }
                    }
                }
                int idx = 0;
                foreach (var item in subPanel.Children.OfType<FrameworkElement>().Where(e => e.DataContext is IGraphViewModel).Select(e => e.DataContext as IGraphViewModel).OrderBy(e => e.Order).ToList())
                {
                    if (item.Order > vm.Order) { break; }
                    idx++;
                }
                var view = (FrameworkElement)Mvvm.ViewHelperFactory.Get(this).Context.GetView<ViewModeDefault, IViewClassFlowchart>(vm);
                view.LayoutUpdated += View_LayoutUpdated;
                view.Loaded += View_Loaded;


                if (view.Parent == null)
                    subPanel?.Children.Insert(idx, view);

            }
        }


        private void UpdateTopValue(Panel p)
        {
            foreach (var c in p.Children.OfType<ConsumableFlowchartView>())
            {
                if (c.DataContext is ConsumableGraphViewModel cgvm)
                    cgvm.Top = c.TranslatePoint(
                        new Point(0, 0), PanelConsumables).Y;
            }

            foreach (var f in p.Children.OfType<ContentControl>())
            {
                if (f.Content is StackPanel sp) UpdateTopValue(sp);
            }

        }

        private void View_LayoutUpdated(object sender, System.EventArgs e)
        {
            UpdateTopValue(PanelConsumables);

        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            ViewLoaded?.Invoke(sender, e);
        }

        private MonographGraphViewModel ViewModel => DataContext as MonographGraphViewModel;


        private static void OnMonographieChanged(DependencyObject source,
                DependencyPropertyChangedEventArgs e)
        {
            //(source as MonographGraphContentView)?.OnMonographieChanged(e);
        }


        //TODO
        //private void OnMonographieChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    var mono = (Monograph)e.NewValue;

        //    if (mono != null && mono.Id != ViewModel?.Model.Id)
        //    {
        //        var vm = new MonographGraphViewModel();
        //        vm.SetModel(mono);
        //        DataContext = vm;
        //    }
        //}



        private void UpdatePanelLayout()
        {
            PanelLinks.UpdateLayout();
            //SetLinksSize();
        }

        public int Intersect()
        {
            return PanelLinks.Children.OfType<LinkFlowchartView>().Sum(e => e.Intersect(PanelLinks));
        }

        public double LinkLength()
        {
            return PanelLinks.Children.OfType<LinkFlowchartView>().Sum(e => e.LinkLength());
        }



        public bool SortPanel(Panel p, FrameworkElement view, Panel otherPanel)
        {
            var n = p.Children.Count;

            UpdatePanelLayout();
            double oldResultL = LinkLength();/*LinkLength();*///Intersect();
            int oldResultN = Intersect();

            var oldPos = p.Children.IndexOf(view);


            var bestPos = oldPos;
            double bestResultL = oldResultL;
            int bestResultN = oldResultN;

            for (var pos = 0; pos < n; pos++)
            {
                p.Children.Remove(view);
                p.Children.Insert(pos, view);

                if (otherPanel != null) SortPanel(otherPanel);

                UpdatePanelLayout();
                var resultN = Intersect();/*LinkLength();*///Intersect();
                var resultL = LinkLength();
                if (resultN < bestResultN)
                {
                    bestResultN = resultN;
                    bestResultL = resultL;
                    bestPos = pos;
                }
                else if (resultN == bestResultN)
                {
                    if (resultL < bestResultL)
                    {
                        bestResultL = resultL;
                        bestPos = pos;
                    }
                }

            }


            if (p.Children.IndexOf(view) != bestPos)
            {
                p.Children.Remove(view);
                p.Children.Insert(bestPos, view);
                if (otherPanel != null) SortPanel(otherPanel);

                //Application.D.Dispatcher.Invoke(
                //    DispatcherPriority.Background,
                //     new System.Threading.ThreadStart(() => { }));
            }

            //Debug.Print(oldResult + " > " + bestResult);

            return oldPos != bestPos;
        }





        public bool SortPanel(Panel p, Panel other = null)
        {
            int c = 1;
            bool changed = false;
            while (c > 0)
            {
                c = 0;
                foreach (var view in p.Children.OfType<FrameworkElement>().ToList())
                {
                    if (SortPanel(p, view, other))
                    {
                        if (other != null)
                            Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Background,
                                 new ThreadStart(() => { }));

                        c++;
                        changed = true;
                    }
                }
            }
            return changed;
        }

        private void SetLinksSize()
        {
            foreach (var link in PanelLinks.Children.OfType<LinkFlowchartView>())
            {
                link.SetSize();
            }
        }




        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SortPanel(PanelSolutions, PanelConsumables);
            RenumConsommables();
            RenumSolutions();
        }

        private int _startIndex = -1;
        private double _startRight = 0.0;
        private Point _startPoint;

        private void DragSolution_Start(ErpDragDrop source)
        {
            var view = source.MouseEventArgs.Source as SolutionFlowchartView;
            var vm = (view?.DataContext as SolutionGraphViewModel);

            var icon = view?.Block.IconPlaceHolder;
            Point p = source.MouseEventArgs.GetPosition(icon);

            if (p.X < 0 || p.Y < 0 || p.X > icon?.ActualWidth || p.Y > icon?.ActualHeight) return;

            _startIndex = PanelSolutions.Children.IndexOf(view);

            _placeHolder.Height = view?.ActualHeight ?? 20;
            _placeHolder.Width = view?.ActualWidth ?? 20;
            SetIndex(_startIndex, _placeHolder);

            _startRight = vm?.Model.Right ?? 0;
            _startPoint = source.MouseEventArgs.GetPosition(GridDragPanelSolutions);

            source.DragShift = new Point(0, 0) - source.MouseEventArgs.GetPosition(view);

            PanelSolutions.Children.Remove(view);

            source.DraggedElement = view;

            if (vm != null)
            { vm.State.Moving = true; }
        }


        private void SetIndex(int i, FrameworkElement e, bool sort = false)
        {
            if (PanelSolutions.Children.Contains(e))
                if (PanelSolutions.Children.IndexOf(e) == i) return;


            if (e.Parent is Panel parent)
            {
                if (parent.Children.Contains(e))
                {
                    if (i > parent.Children.IndexOf(e))
                        i--;

                    parent.Children.Remove(e);
                }
            }

            if (i >= 0)
                PanelSolutions.Children.Insert(i, e);

            SetLinksSize();
            if (sort)
            {
                SortPanel(PanelConsumables);
                RenumConsommables();
            }
        }


        private readonly FrameworkElement _placeHolder = new FrameworkElement();

        private int GetIndex(ErpDragDrop s)
        {
            int i = 0;
            foreach (FrameworkElement element in PanelSolutions.Children)
            {
                if (s.HitTest(element)) break;
                i++;
            }
            return i;
        }

        private void DragSolution_Drop(ErpDragDrop s)
        {
            if (s.DraggedElement?.DataContext is SolutionGraphViewModel vm)
            {
                vm.State.Moving = false;
                vm.Top = s.MouseEventArgs.GetPosition(GridDragPanelSolutions).Y + s.DragShift.Y;
                vm.Model.Right =
                    Math.Max(_startRight + (_startPoint - s.MouseEventArgs.GetPosition(GridDragPanelSolutions)).X, 0);

                s.DraggedElement.SetBinding(SolutionFlowchartView.MarginProperty, "Margin");
            }


            var i = GetIndex(s);

            SetIndex(i > PanelSolutions.Children.Count ? _startIndex : i, s.DraggedElement);

            SetIndex(-1, _placeHolder, true);

            RenumSolutions();
        }

        private void RenumSolutions()
        {
            var i = 0;
            foreach (FrameworkElement element in PanelSolutions.Children)
            {
                if (element.DataContext is SolutionGraphViewModel e)
                {
                    e.Model.Order = i;
                    i++;
                }
            }
        }
        private void RenumConsommables()
        {
            int i = 0;
            foreach (FrameworkElement element in PanelConsumables.Children)
            {
                if (element.DataContext is ConsumableGraphViewModel e)
                {
                    e.Model.Order = i;
                    i++;
                }
            }
        }
        private void RenumTests()
        {
            int i = 0;
            foreach (FrameworkElement element in PanelTests.Children)
            {
                if (element.DataContext is TestGraphViewModel e)
                {
                    e.Model.Order = i;
                    i++;
                }
            }
        }

        private void DragSolution_Move(ErpDragDrop s)
        {
            int i = GetIndex(s);
            if (s.DraggedElement?.DataContext is SolutionGraphViewModel vm)
            {

                s.DraggedElement.Margin = new Thickness(
                    0, //0,
                    s.DraggedElement.Margin.Top,
                    Math.Max(vm.Right + (_startPoint - s.MouseEventArgs.GetPosition(GridDragPanelSolutions)).X, 0),
                    0);
            }

            SetLinksSize();

            s.Positioned = true;
        }


        private void UIElement_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }


        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer viewer && !e.Handled)

            {

                e.Handled = true;

                var eventArg =
                    new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                    {
                        RoutedEvent = UIElement.MouseWheelEvent,
                        Source = viewer
                    };


                var parent = ((Control)sender).Parent as UIElement;

                parent?.RaiseEvent(eventArg);

            }
        }

        private void ScrollViewer_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Select(null);
        }

    }
}
