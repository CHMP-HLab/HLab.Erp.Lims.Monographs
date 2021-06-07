using System.Windows.Controls;
using HLab.Erp.Core;
using HLab.Erp.Core.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Tools.Details
{
    /// <summary>
    /// Logique d'interaction pour DetailView.xaml
    /// </summary>
    public partial class DetailPanelView : UserControl, IViewClassAnchorable, IView<ViewModeDefault, DetailsPanelViewModel>
    {
        public DetailPanelView()
        {
            InitializeComponent();
        }

        public string ContentId => GetType().Name;
    }
}
