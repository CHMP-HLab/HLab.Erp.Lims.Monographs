using System.Windows.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Monographs.Module.Classes.Monographs.MonographsTreeView.Views
{
    /// <summary>
    /// Logique d'interaction pour PharmacopeeContentView.xaml
    /// </summary>
    public partial class PharmacopoeiaContentView : UserControl, IView<ViewModeContent,PharmacopoeiaTreeElementViewModel>
    {
        public PharmacopoeiaContentView()
        {
            InitializeComponent();
        }
    }
}
