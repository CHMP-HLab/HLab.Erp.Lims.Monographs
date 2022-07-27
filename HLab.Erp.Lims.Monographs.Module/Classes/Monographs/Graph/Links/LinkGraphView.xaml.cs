using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using HLab.Erp.Data;
using HLab.Erp.Lims.Monographs.Module.Geo;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Extensions;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph.Links
{
    /// <summary>
    /// Logique d'interaction pour LinkGraph.xaml
    /// </summary>
    /// 

    public partial class LinkFlowchartView : UserControl
        , IView<ViewModeDefault,LinkGraphViewModel>
        , IViewClassFlowchart
    {
        public LinkFlowchartView()
        {
            InitializeComponent();
            DataContextChanged += LinkGraphView_DataContextChanged;
            LayoutUpdated += LinkGraphView_LayoutUpdated;
//            ViewModeContext.ViewDataContextChanged += ViewModeContext_ViewDataContextChanged; ;
            Loaded += LinkGraphView_Loaded;
        }

        void LinkGraphView_Loaded(object sender, RoutedEventArgs e)
        {
            this.FindVisualParent<MonographFlowchartView>().ViewLoaded += LinkGraphView_ViewLoaded; ;
        }

        void LinkGraphView_ViewLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                var model = ((element.DataContext as IViewModel)?.Model as IEntity);
                //var right = ((ViewModel.RightViewModel as IViewModel)?.Model as IEntity);
                //var left = ((ViewModel.LeftViewModel as IViewModel)?.Model as IEntity);

                //if (ReferenceEquals(left, model))
                //{
                //    LeftView = element;
                //    SetSize();
                //}

                //if (ReferenceEquals(right, model))
                //{
                //    RightView = element;
                //    SetSize();
                //}
            }
        }

        //private void ViewModeContext_ViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    if (ViewModel.LeftViewModel == e.NewValue)
        //    {
        //        LeftView = sender as FrameworkElement;
        //        SetSize();
        //    }
        //    if (ViewModel.RightViewModel == e.NewValue)
        //    {
        //        RightView = sender as FrameworkElement;
        //        SetSize();
        //    }
        //}

        bool _loaded = false;

        void LinkGraphView_LayoutUpdated(object sender, EventArgs e)
        {
            if (!_loaded && (ActualHeight > 0 || ActualWidth > 0))
            {
                LeftViewModelChanged();
                RightViewModelChanged();
                _loaded = true;
            }
        }

        void LinkGraphView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                if (e.OldValue is LinkGraphViewModel vm)
                {
                    //vm.Root.
                    //ViewModeContext.ViewDataContextChanged -= ViewModeContext_ViewDataContextChanged;
                    vm.PropertyChanged -= Vm_PropertyChanged;
                }
            }

            if (e.NewValue != null)
            {
                if (e.NewValue is LinkGraphViewModel vm)
                {

                    //vm.Root.
                    //ViewModeContext.ViewDataContextChanged += ViewModeContext_ViewDataContextChanged; ;
                    vm.PropertyChanged += Vm_PropertyChanged;
                }
            }
            SetSize();
        }


        FrameworkElement LeftView { get; set; }

        FrameworkElement RightView { get; set; }

        void LeftViewModelChanged()
        {
            //if (ViewModel.LeftViewModel == null) return;

            //var newLeft = ViewModel.LeftViewModel.GetActualView(this.FindParent<MonographFlowchartView>());

            ////if (!ReferenceEquals(LeftView,newLeft))
            //{
            //    if (LeftView != null)
            //    {
            //        LeftView.LayoutUpdated -= SetSize;
            //        LeftView.SizeChanged -= SetSize;
            //    }
            //    LeftView = newLeft;
            //    if (LeftView != null)
            //    {
            //        LeftView.LayoutUpdated += SetSize;
            //        LeftView.SizeChanged += SetSize;
            //    }
            //}
            //SetSize();
        }

        void RightViewModelChanged()
        {
            //if (ViewModel.RightViewModel == null) return;

            //var newRight = ViewModel.RightViewModel.GetActualView(this.FindParent<MonographFlowchartView>());

            ////if (!ReferenceEquals(RightView, newRight))
            //{
            //    if (RightView != null)
            //    {
            //        RightView.LayoutUpdated -= SetSize;
            //        RightView.SizeChanged -= SetSize;
            //    }
            //    RightView = newRight;
            //    if (RightView != null)
            //    {
            //        RightView.LayoutUpdated += SetSize;
            //        RightView.SizeChanged += SetSize;
            //    }
            //}
            //SetSize();
        }

        void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Dispatcher.Invoke(
                () =>
                {
                    switch (e.PropertyName)
                    {
                        case "LeftViewModel":
                            LeftViewModelChanged();
                            break;
                        case "RightViewModel":
                            RightViewModelChanged();
                            break;
                    }
                }
                );
        }

        LinkGraphViewModel ViewModel => DataContext as LinkGraphViewModel;

        public static readonly DependencyProperty ContainerPanelProperty = DependencyProperty.Register(nameof(ContainerPanel), typeof(Panel), typeof(LinkFlowchartView), new FrameworkPropertyMetadata(OnChanged));

        public Panel ContainerPanel
            //{ get { return (Panel)GetValue(ContenerProperty); } set { SetValue(ContenerProperty, value); } }
            => VisualTreeHelper.GetParent(this) as Panel;

        static void OnChanged(DependencyObject src,
            DependencyPropertyChangedEventArgs e)
        {
            (src as LinkFlowchartView)?.OnChanged(e);
        }

        public bool Intersect(LinkFlowchartView other)
        {
            //            SetSize();
            var s1 = new Segment(StartPoint, EndPoint);
            var s2 = new Segment(other.StartPoint, other.EndPoint);
            return s1.Intersect(s2) != null;
        }

        public double LinkLength()
        {
            var s1 = new Segment(StartPoint, EndPoint);
            return s1.Size;
        }

        public int Intersect(Panel p)
        {
            var links = p.Children.OfType<LinkFlowchartView>().Where(e => !ReferenceEquals(e,this));

            var i = links.Count(Intersect);

            return i;
        }


        void OnChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldgrid = e.OldValue as FrameworkElement;
            var newgrid = e.NewValue as FrameworkElement;

            if (oldgrid != null)
            {
                oldgrid.Loaded -= SetSize; ;
                oldgrid.SizeChanged -= SetSize;
            }
            if (newgrid != null)
            {
                newgrid.Loaded += SetSize; ;
                newgrid.SizeChanged += SetSize;
            }
        }

        void SetSize(object sender, RoutedEventArgs e)
        {
            SetSize();
        }

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }



        public void SetSize(object sender, EventArgs e) { SetSize(); }

        public void SetSize()
        {
            if (LeftView==null) return;
            if (RightView==null) return;


            StartPoint = LeftView.TranslatePoint(new Point(LeftView.ActualWidth, LeftView.ActualHeight / 2), ContainerPanel);
            EndPoint = RightView.TranslatePoint(new Point(0, RightView.ActualHeight / 2), ContainerPanel);

            var w = (StartPoint.X < EndPoint.X)? (EndPoint.X - StartPoint.X)/3 : (StartPoint.X - EndPoint.X)/2;
            var h = (StartPoint.X < EndPoint.X) ? 0 : (EndPoint.Y - StartPoint.Y) / 4;

            Figure.StartPoint = StartPoint;

            ((QuadraticBezierSegment)Figure.Segments[0]).Point1 = new Point(StartPoint.X + w, StartPoint.Y+h);
            ((QuadraticBezierSegment)Figure.Segments[0]).Point2 = new Point((StartPoint.X + EndPoint.X) / 2, (StartPoint.Y + EndPoint.Y) / 2);
            ((QuadraticBezierSegment)Figure.Segments[1]).Point1 = new Point(EndPoint.X - w, EndPoint.Y-h);

            ((QuadraticBezierSegment)Figure.Segments[1]).Point2 = EndPoint;

            Label.Margin = new Thickness(
                ((StartPoint.X + EndPoint.X - Label.ActualWidth) / 2),
                (StartPoint.Y + EndPoint.Y - Label.ActualHeight) / 2,
                0, 0
                );

        }

        void Path_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Selected = true;
        }
    }
}
