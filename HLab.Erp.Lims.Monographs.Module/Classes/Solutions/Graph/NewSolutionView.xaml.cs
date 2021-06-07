using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph
{
    /// <summary>
    /// Logique d'interaction pour NewTestView.xaml
    /// </summary>
    public partial class NewSolutionView : UserControl
        , IView<ViewModeDefault,NewSolutionViewModel>
        , IViewClassFlowchart
    {
        public NewSolutionView()
        {
            InitializeComponent();
        }
    }
}
