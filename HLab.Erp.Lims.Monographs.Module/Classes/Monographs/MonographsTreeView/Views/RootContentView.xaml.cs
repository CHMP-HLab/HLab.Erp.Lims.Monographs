using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class RootContentView : UserControl, IView<ViewModeContent, MonographsListViewModel>
    {
        public RootContentView()
        {
            InitializeComponent();
        }
    }
}
