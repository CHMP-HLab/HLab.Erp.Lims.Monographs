using System.Windows.Controls;
using HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Monographs;
using HLab.Erp.Lims.Monographs.Module.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Solutions.Monographie
{
    /// <summary>
    /// Logique d'interaction pour MonographieSolutionView.xaml
    /// </summary>
    public partial class MonographSolutionView : UserControl
        , IView<ViewModeDefault,MonographSolutionViewModel>
        , IViewClassDetail
    {
        public MonographSolutionView()
        {
            InitializeComponent();
        }
    }
}
