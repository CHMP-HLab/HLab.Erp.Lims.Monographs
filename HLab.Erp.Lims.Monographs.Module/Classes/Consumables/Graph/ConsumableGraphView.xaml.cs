using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Graph
{
    /// <summary>
    /// Logique d'interaction pour SolutionMonographieView.xaml
    /// </summary>
    public partial class ConsumableFlowchartView : UserControl
        , IView<ViewModeDefault,ConsumableGraphViewModel> 
        , IViewClassFlowchart
    {
        public ConsumableFlowchartView()
        {
            InitializeComponent();
        }
    }
}
