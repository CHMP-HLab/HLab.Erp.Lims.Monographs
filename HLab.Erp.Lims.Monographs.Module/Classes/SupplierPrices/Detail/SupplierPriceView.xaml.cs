using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.SupplierPrices.Detail
{
    /// <summary>
    /// Logique d'interaction pour SupplierPrice.xaml
    /// </summary>
    
    public partial class SupplierPriceView : UserControl, IView<ViewModeList,SupplierPriceViewModel>
    {
        public SupplierPriceView()
        {
            InitializeComponent();
        }
    }
}
