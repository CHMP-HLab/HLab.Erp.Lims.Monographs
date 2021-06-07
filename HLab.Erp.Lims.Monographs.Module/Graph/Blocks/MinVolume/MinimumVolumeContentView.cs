using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.MinVolume
{
    /// <summary>
    /// Logique d'interaction pour DissolutionContentBlock.xaml
    /// </summary>
    public partial class MinimumVolumeContentView : UserControl,
        IView<ViewModeDefault, MinimumVolumeBlock>,
        IView<ViewModeEdit, MinimumVolumeBlock>, IViewClassBlockContent
    {
        public MinimumVolumeContentView()
        {
            InitializeComponent();
        }
    }
}
