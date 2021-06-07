using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks
{
    /// <summary>
    /// Logique d'interaction pour PinValueView.xaml
    /// </summary>
    public partial class PinValueView : UserControl, 
        IView<ViewModeDefault,PinValueViewModel>,
        IView<ViewModeEdit,PinValueViewModel>, 
        IViewClassPinValue
    {
        public PinValueView()
        {
            InitializeComponent();
        }
    }
}
