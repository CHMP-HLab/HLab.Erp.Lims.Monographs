using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{
    /// <summary>
    /// Logique d'interaction pour GraphValueEditView.xaml
    /// </summary>
    public partial class GraphValueEditView : UserControl,
        IView<ViewModeEdit,MonographValueViewModel>
    {
        public GraphValueEditView()
        {
            InitializeComponent();
        }
    }
}
