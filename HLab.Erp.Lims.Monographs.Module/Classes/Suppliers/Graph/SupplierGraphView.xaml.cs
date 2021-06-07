using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Suppliers.Graph
{
    /// <summary>
    /// Logique d'interaction pour FournisseurGraphView.xaml
    /// </summary>
    public partial class SupplierFlowchartView : UserControl, 
        IView<ViewModeDefault, SupplierGraphViewModel>, IViewClassFlowchart
    {
        public SupplierFlowchartView()
        {
            InitializeComponent();
        }
    }
}
