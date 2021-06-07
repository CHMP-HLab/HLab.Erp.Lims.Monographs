using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Solution
{
    /// <summary>
    /// Logique d'interaction pour DissolutionContentBlock.xaml
    /// </summary>
    public partial class DissolutionContentBlock : UserControl,
        IView<ViewModeDefault, SolutionBlock>,
        IView<ViewModeEdit, SolutionBlock>, IViewClassBlockContent
    {
        public DissolutionContentBlock()
        {
            InitializeComponent();
        }
    }
}
