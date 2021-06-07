using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Injection.Gradients
{
    /// <summary>
    /// Logique d'interaction pour GradiantPreview.xaml
    /// </summary>
    public partial class GradientPreview : UserControl,
        IView<ViewModeDefault,GradientViewModel>,
        IView<ViewModeEdit,GradientViewModel>,
        IViewClassBlockContent
    {
        public GradientPreview()
        {
            InitializeComponent();
        }
    }
}
