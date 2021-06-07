using System.Windows.Controls;
using HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Graph;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.TestClasses.Graph
{
    /// <summary>
    /// Logique d'interaction pour NewSolutionView.xaml
    /// </summary>
    public partial class NewAssayView : UserControl, 
        IView<ViewModeDefault,NewSolutionViewModel>,IViewClassFlowchart
    {
        public NewAssayView()
        {
            InitializeComponent();
        }
    }
}
