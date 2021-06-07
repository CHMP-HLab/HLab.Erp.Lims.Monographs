using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Gradients
{
    /// <summary>
    /// Logique d'interaction pour GradiantPreview.xaml
    /// </summary>
    public partial class GradientPreview : UserControl,
        IView<ViewModePreview,GradientViewModel>, IViewClassFlowchart
    {
        public GradientPreview()
        {
            InitializeComponent();
        }
    }
}
