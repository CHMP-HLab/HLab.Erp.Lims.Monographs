using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.DebugTools
{
    /// <summary>
    /// Logique d'interaction pour DebugView.xaml
    /// </summary>
    public partial class DebugView : UserControl, IView<ViewModeDefault,DebugViewModel>
    {
        public DebugView()
        {
            InitializeComponent();
            //DataContext = new DebugViewModel();
        }
    }
}
