using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Graph
{
    /// <summary>
    /// Logique d'interaction pour TestTypeGraphView.xaml
    /// </summary>
    public partial class TestClassFlowchartView : UserControl
        , IView<ViewModeDefault, TestClassGraphViewModel>
        , IViewClassFlowchart
    {
        public TestClassFlowchartView()
        {
            InitializeComponent();
        }
    }
}
