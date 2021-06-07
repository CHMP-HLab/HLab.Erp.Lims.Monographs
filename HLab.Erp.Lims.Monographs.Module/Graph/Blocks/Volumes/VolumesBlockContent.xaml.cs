using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Volumes
{
    /// <summary>
    /// Logique d'interaction pour UVolumesBlockContent.xaml
    /// </summary>
    public partial class VolumesBlockContent : UserControl,
        IView<ViewModeDefault, VolumesBlock>,
        IView<ViewModeEdit, VolumesBlock>,
        IViewClassBlockContent
    {
        public VolumesBlockContent()
        {
            InitializeComponent();
        }
    }
}
