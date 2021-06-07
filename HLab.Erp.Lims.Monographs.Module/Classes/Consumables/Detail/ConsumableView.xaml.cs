using System.Windows.Controls;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Consumables.Detail
{
    /// <summary>
    /// Logique d'interaction pour ConsumableView.xaml
    /// </summary>
    public partial class ConsumableView : UserControl
        , IView<ViewModeDefault,ConsumableViewModel>
        , IViewClassDetail
    {
        public ConsumableView()
        {
            InitializeComponent();
        }
    }
}
