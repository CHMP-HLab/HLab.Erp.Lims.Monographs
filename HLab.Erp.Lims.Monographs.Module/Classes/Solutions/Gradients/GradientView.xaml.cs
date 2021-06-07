using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Gradients
{
    /// <summary>
    /// Logique d'interaction pour GradientView.xaml
    /// </summary>
    public partial class GradientView : UserControl
        , IView<ViewModeDefault,GradientViewModel>
        , IViewClassDetail
    {
        public GradientView()
        {
            InitializeComponent();

            //view.IsLiveSorting = true;             

            // TODO : Chart.DataClick += Chart_DataClick;
        }

        private GradientViewModel ViewModel => DataContext as GradientViewModel;


        private GradientLineViewModel _movingPoint;
        private Point _startPoint;
        private double _startY;

        private void Chart_DataClick(object sender, object chartPoint)
        {
            //TODO : _movingPoint = (chartPoint.Instance as GradientLineViewModel);
            //Chart.MouseMove += Chart_MouseMove;
            //Chart.MouseLeftButtonUp += Chart_MouseLeftButtonUp;
            //_startPoint = Mouse.GetPosition(Chart);
            //_startY = _movingPoint.RatioPCent;
        }

        private void Chart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _movingPoint = null;
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            if (_movingPoint == null) return;

            //Point p = e.GetPosition(Chart);

            //TODO

            //double ratio = Math.Round(ChartFunctions.FromPlotArea(p.Y, AxisOrientation.Y, Chart.Model),0);

            //double time = Math.Round(ChartFunctions.FromPlotArea(p.X, AxisOrientation.X, Chart.Model),0);

            //_movingPoint.RatioPCent = (ratio > 100)?100:(ratio < 0)?0: ratio;
            //_movingPoint.Time = time;
        }

    }
}
