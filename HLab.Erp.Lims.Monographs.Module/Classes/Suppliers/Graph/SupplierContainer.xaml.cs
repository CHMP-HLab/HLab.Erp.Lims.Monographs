using System.Windows.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Flowchart;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Suppliers.Graph
{
    /// <summary>
    /// Logique d'interaction pour FournisseurContainer.xaml
    /// </summary>
    public partial class FournisseurContainer : UserControl
//        , IView<ViewModeDefault,SupplierGraphViewModel>
        , IView<ViewModeDefault, SupplierGraphViewModel>
        , IViewClassGraphContainer
    {
        public FournisseurContainer()
        {
            InitializeComponent();
        }
    }
}
