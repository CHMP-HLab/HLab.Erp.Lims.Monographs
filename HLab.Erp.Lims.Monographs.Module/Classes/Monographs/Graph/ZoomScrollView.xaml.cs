using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour ZoomScrollView.xaml
    /// </summary>
    /// 
    [ContentProperty("ZoomedContent")]
    public partial class ZoomScrollView : UserControl
    {
        public ZoomScrollView()
        {
            InitializeComponent();
        }

        public object ZoomedContent
        {
            get => (object)GetValue(ZoomedContentProperty);
            set => SetValue(ZoomedContentProperty, value);
        }
        public static readonly DependencyProperty ZoomedContentProperty =
            DependencyProperty.Register(nameof(ZoomedContent), typeof(object), typeof(ZoomScrollView),
                new PropertyMetadata(null));

        //public Grid Grid => (Grid)GetTemplateChild("Grid");
        //public ScaleTransform ScaleTransform => (ScaleTransform)GetTemplateChild("ScaleTransform");
        //public TranslateTransform TranslateTransform => (TranslateTransform)GetTemplateChild("TranslateTransform");
        //public ScrollViewer ScrollViewer => (ScrollViewer)GetTemplateChild("ScrollViewer");

        void Grid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var element = sender as UIElement;
            var p1 = e.GetPosition(Grid);
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);

            ScaleTransform.ScaleX *= scale;
            ScaleTransform.ScaleY *= scale;

            element?.UpdateLayout();
            var p2 = e.GetPosition(Grid);

            var pp1 = Grid.TranslatePoint(p1, ScrollViewer);
            var pp2 = Grid.TranslatePoint(p2, ScrollViewer);

            var x = ScrollViewer.HorizontalOffset;
            var y = ScrollViewer.VerticalOffset;
            ScrollViewer.ScrollToHorizontalOffset(x + pp1.X - pp2.X);
            ScrollViewer.ScrollToVerticalOffset(y + pp1.Y - pp2.Y);

            //string S(Point p0) => Math.Round(p0.X) + " : " + Math.Round(p0.Y);

           // ViewModel.DebugText = S(p) + " <==> " + S(p1) + " => " + S(p2) + " => " + S(p3) + " --- " + Math.Round(MousePanel.ActualWidth) + " : " + Math.Round(MousePanel.ActualHeight);

            e.Handled = true;
        }

        void Grid_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            throw new NotImplementedException();
        }

        Point _movePoint;
        bool _moved = false;

        void Grid_OnMouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(sender as IInputElement);
            //            ViewModel.DebugText = p.X + "-" + p.Y;


            if (sender is UIElement sw && sw.IsMouseCaptured)
            {
                var v = e.GetPosition(sw) - _movePoint;
                var x = ScrollViewer.ContentHorizontalOffset;
                var y = ScrollViewer.ContentVerticalOffset;

                ScrollViewer.ScrollToHorizontalOffset(x - v.X);
                ScrollViewer.ScrollToVerticalOffset(y - v.Y);
                _movePoint = e.GetPosition(sw);
                _moved = true;
                e.Handled = true;
            }
        }

        void Grid_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((e.ChangedButton == MouseButton.Middle || e.ChangedButton == MouseButton.Right) && sender is UIElement element)
            {
                _movePoint = e.GetPosition(element);
                element.CaptureMouse();
                _moved = false;
                e.Handled = true;
            }
        }

        void Grid_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement element && element.IsMouseCaptured)
            {
                element.ReleaseMouseCapture();
                e.Handled = _moved;
                _moved = false;
            }
        }
    }
}
