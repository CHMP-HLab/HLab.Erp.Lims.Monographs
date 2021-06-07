using System.Windows.Controls;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Monographs
{
    /// <summary>
    /// Logique d'interaction pour MonographConsumableView.xaml
    /// </summary>
    /// 
    public partial class MonographConsumableView : UserControl
        , IView<ViewModeDefault,MonographConsumableViewModel>
        , IViewClassDetail
    {
        public MonographConsumableView()
        {
            InitializeComponent();
        }
    }
}
