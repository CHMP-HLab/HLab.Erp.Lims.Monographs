using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.Graph
{
    /// <summary>
    /// Logique d'interaction pour MonographieGraph.xaml
    /// </summary>
    public partial class MonographGraphContainerView : UserControl
        , IView<ViewModeDefault, MonographGraphViewModel>
        , IViewClassGraphContainer
    {
        public MonographGraphContainerView()
        {
            InitializeComponent();
        }

    }    
}
