using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Graph
{
    /// <summary>
    /// Logique d'interaction pour ValueView.xaml
    /// </summary>
    public partial class GraphValueView : UserControl,
        IView<ViewModeDefault,MonographValueViewModel>,
        IView<ViewModeEdit, MonographValueViewModel>
    {
        public GraphValueView()
        {
            InitializeComponent();
        }
    }
}
