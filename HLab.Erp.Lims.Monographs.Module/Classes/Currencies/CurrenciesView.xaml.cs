using System.Windows.Controls;
using HLab.Erp.Core;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Currencies
{
    /// <summary>
    /// Logique d'interaction pour Currency.xaml
    /// </summary>
    public partial class CurrenciesView : UserControl , IView<ViewModeDefault,CurrenciesToolViewModel> , IViewClassAnchorable
    {
        public CurrenciesView()
        {
            InitializeComponent();
        }
        public string ContentId => GetType().Name;
    }
}
