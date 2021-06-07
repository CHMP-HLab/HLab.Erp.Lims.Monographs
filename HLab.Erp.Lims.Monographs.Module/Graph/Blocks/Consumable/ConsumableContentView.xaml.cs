using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks.Consumable
{
    /// <summary>
    /// Logique d'interaction pour ConsumableContentView.xaml
    /// </summary>
    public partial class ConsumableContentView : UserControl, 
        IView<ViewModeDefault, ConsumableBlock>, 
        IViewClassBlockContent
    {
        public ConsumableContentView()
        {
            InitializeComponent();
        }
    }
}
