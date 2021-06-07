using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart.Models;
using HLab.Mvvm.Flowchart.Views;

namespace HLab.Erp.Lims.Monographs.Module.Graph.Blocks
{
    /// <summary>
    /// Logique d'interaction pour DefaultContentView.xaml
    /// </summary>
    public partial class DefaultContentView : UserControl, 
        IView<ViewModeDefault,IPin>,
        IView<ViewModeEdit,IPin>, 
        IViewClassPinContent
    {
        public DefaultContentView()
        {
            InitializeComponent();
        }
    }
}
