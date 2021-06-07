using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Volumes
{
    /// <summary>
    /// Logique d'interaction pour VolumesValueView.xaml
    /// </summary>
    public partial class VolumesValueView : UserControl, IView<ViewModeDefault, InputPinVolumesViewModel>, IViewClassPinContent
    {
        public VolumesValueView()
        {
            InitializeComponent();
        }
    }
}
