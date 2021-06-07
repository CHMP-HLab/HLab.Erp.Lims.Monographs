using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour Anchor.xaml
    /// </summary>
    public partial class Anchor : UserControl
    {
        public Anchor()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty DragCanvasProperty = DependencyProperty.Register(nameof(DragCanvas),
            typeof(Canvas), typeof(Anchor), new FrameworkPropertyMetadata(OnDragCanvasChanged));

        public static readonly DependencyProperty EnabledProperty = DependencyProperty.Register(nameof(Enabled),
            typeof(bool), typeof(Anchor), new FrameworkPropertyMetadata(OnEnabledChanged));

        public static readonly DependencyProperty HighlightProperty = DependencyProperty.Register(nameof(Highlight),
            typeof(bool), typeof(Anchor), new FrameworkPropertyMetadata(OnHighlightChanged));

        public static readonly DependencyProperty AnchorClassProperty = DependencyProperty.Register(nameof(AnchorClass),
            typeof(string), typeof(Anchor));

        public string AnchorClass
        {
            get => (string)GetValue(AnchorClassProperty); set => SetValue(AnchorClassProperty, value);
        }

        public Canvas DragCanvas
        {
            get => (Canvas)GetValue(DragCanvasProperty); set => SetValue(DragCanvasProperty, value);
        }

        public bool Enabled
        {
            get => (bool)GetValue(EnabledProperty); set => SetValue(EnabledProperty, value);
        }
        public bool Highlight
        {
            get => (bool)GetValue(HighlightProperty); set => SetValue(HighlightProperty, value);
        }
        private static void OnDragCanvasChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnEnabledChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnHighlightChanged(DependencyObject source,
            DependencyPropertyChangedEventArgs e)
        {
            (source as Anchor)?.OnHighlightChanged(e);
        }

        public void OnHighlightChanged(DependencyPropertyChangedEventArgs e)
        {
            bool v = (bool)e.NewValue;
            if (v)
                Border.Background = new SolidColorBrush(Colors.CornflowerBlue);
            else
                Border.Background = new SolidColorBrush(Colors.White);

        }


        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (Enabled)
            {
                Border.Background = new SolidColorBrush(Colors.CornflowerBlue);
            }
        }

        private void Border_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (Enabled)
            {
                Border.Background = new SolidColorBrush(Colors.White);
            }
        }

        private Path _line = null;
        private Point _point;

        private void SetLine(Point startPoint, Point endPoint)
        {
            if (_line == null)
            {
                _line = new Path
                {
                    StrokeThickness = 5,
                    IsHitTestVisible = false,
                    Stroke = new SolidColorBrush(Colors.Red),
                    Data = new PathGeometry
                    {
                        Figures =
                        {
                            new  PathFigure
                            {
                                Segments = {new QuadraticBezierSegment(), new QuadraticBezierSegment()},
                            }
                        }
                    }
                };
            }

            PathFigure pf = (_line.Data as PathGeometry).Figures[0];

            //var w = (startPoint.X < endPoint.X) ? (endPoint.X - startPoint.X) / 3 : (startPoint.X - endPoint.X) / 2;
            //var h = (startPoint.X < endPoint.X) ? 0 : (endPoint.Y - startPoint.Y) / 4;
            var w = (endPoint.X - startPoint.X) / 3;
            var h = 0;
            pf.StartPoint = startPoint;

            ((QuadraticBezierSegment)pf.Segments[0]).Point1 = new Point(startPoint.X + w, startPoint.Y + h);
            ((QuadraticBezierSegment)pf.Segments[0]).Point2 = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
            ((QuadraticBezierSegment)pf.Segments[1]).Point1 = new Point(endPoint.X - w, endPoint.Y - h);

            ((QuadraticBezierSegment)pf.Segments[1]).Point2 = endPoint;
        }


        private void Border_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DragCanvas == null) return;

            if (!(e.Source is FrameworkElement t)) return;

            var p = TranslatePoint(new Point(Width / 2, Height / 2), DragCanvas);

            _point = p;
            SetLine(p, p);

            if (!DragCanvas.Children.Contains(_line))
                DragCanvas.Children.Add(_line);

            t.CaptureMouse();
        }

        private void Border_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var t = e.Source as FrameworkElement;
            t?.ReleaseMouseCapture();

            if (_line != null && DragCanvas.Children.Contains(_line))
            {
                DragCanvas.Children.Remove(_line);
            }
            _line = null;

            if (_dest != null)
                Connect(_dest);
            //e.Handled = true;
        }

        private void Connect(Anchor dest)
        {
            var dstViewModel = dest.DataContext as IGraphViewModel;
            var dstClass = dest.AnchorClass;

            (DataContext as IGraphViewModel)?.ConnectTo(AnchorClass,dstViewModel,dstClass);
        }

        public bool IsConnectable(IGraphViewModel othermodel, string anchorClass)
        {
            if (DataContext is IGraphViewModel thismodel)
            {
                return thismodel.IsConnectable(AnchorClass, othermodel,anchorClass);
            }
            return false;
        }

        private Anchor _dest = null;
        

        private void Border_OnMouseMove(object sender, MouseEventArgs e)
        {

            var p = e.GetPosition(DragCanvas);
            if (_line == null) return;

            DragCanvas.Children.Remove(_line);

            DependencyObject main = this;
            while (true)
            {
                main = VisualTreeHelper.GetParent(main);
                if (main is Window) break;
            }

//            var visual = main as Visual;

            var result = VisualTreeHelper.HitTest((Window)main, e.GetPosition((Window)main));
            var dest = result?.VisualHit;


            var t = dest?.GetType()?.Name ?? "-" ;

            while (dest != null && !(dest is Anchor))
            {
                dest = VisualTreeHelper.GetParent(dest);
                t += "/" + dest?.GetType()?.Name ?? "-";
            }
            Debug.Print(t);

            if (dest != null)
            {
                if (DataContext is IGraphViewModel model)
                {
                    var anchor = dest as Anchor;

                    if (!ReferenceEquals(dest,this))
                    {
                        if (anchor.IsConnectable(model,AnchorClass))
                        {
                            _dest = anchor;
                            _dest.Highlight = true;
                        }                        
                    }                    
                }
            }
            else
            {
                if (_dest != null)
                {
                    _dest.Highlight = false;
                    _dest = null;
                }
            }

            SetLine(_point, p);
            DragCanvas.Children.Add(_line);
        }
    }
}
